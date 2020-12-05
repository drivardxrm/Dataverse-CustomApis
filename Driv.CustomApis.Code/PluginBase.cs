using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;


/// <summary>
/// Base class for Dynamics 365 Plugin Developement.
/// </summary>    
public abstract class PluginBase : IPlugin
{
    public class LocalPluginContext
    {
        #region Contructor

        private LocalPluginContext()
        {
        }

        internal LocalPluginContext(IServiceProvider serviceProvider)
        {
            // Important to be able to impersonify
            ServiceProvider = serviceProvider;
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }


            // Obtain the execution context service from the service provider.
            PluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtain the tracing service from the service provider.
            TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the Organization Service factory service from the service provider
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            // Use the factory to generate the Organization Service.
            OrganizationService = factory.CreateOrganizationService(PluginExecutionContext.UserId);
        }

        #endregion



        internal IServiceProvider ServiceProvider { get; }

        internal IOrganizationService OrganizationService { get; }

        internal IPluginExecutionContext PluginExecutionContext { get; }

        internal ITracingService TracingService { get; }


        internal Guid UserId => PluginExecutionContext.UserId;

        internal int UserLocaleId
        {
            get
            {
                var fetch = new FetchExpression("<fetch>" +
                            "<entity name = 'usersettings'>" +
                            "<attribute name = 'systemuserid'/>" +
                            "<attribute name = 'uilanguageid'/>" +
                            "<filter>" +
                            $"<condition attribute = 'systemuserid' operator= 'eq' value = '{UserId}'/>" +
                            "</filter>" +
                            "</entity>" +
                            "</fetch>");

                var results = OrganizationService.RetrieveMultiple(fetch);
                if (results.Entities.Count > 0)
                {
                    return (int)results[0].Attributes["uilanguageid"];
                }

                return 1033; 
            }
        }

        internal Locale UserLocale
        {
            get
            {
                switch (UserLocaleId)
                {
                    case 1036:
                        return Locale.French;
                    default:
                        return Locale.English;
                }
            }
        }

        internal InvalidPluginExecutionException LocalizedError(string msgFrench, string msgEnglish)
            => new InvalidPluginExecutionException(UserLocale == Locale.French ? msgFrench : msgEnglish); 

        internal EventOperation Message
        {
            get
            {
                Enum.TryParse(PluginExecutionContext.MessageName, out EventOperation message);
                return message;
            }
        }

        internal void Trace(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || TracingService == null)
            {
                return;
            }

            TracingService.Trace(PluginExecutionContext == null
                ? message
                : $"{message}, Correlation Id: {PluginExecutionContext.CorrelationId}, Initiating User: {PluginExecutionContext.InitiatingUserId}");
        }

        #region InputParameters/OutputParameters

        internal EntityReference Assignee
        {
            get
            {
                if (PluginExecutionContext.InputParameters.Contains("Assignee") &&
                    PluginExecutionContext.InputParameters["Assignee"] is EntityReference)
                {
                    return (EntityReference)PluginExecutionContext.InputParameters["Assignee"];
                }

                return null;
            }
        }

        internal EntityReference Record
        {
            get
            {
                if (PluginExecutionContext.InputParameters.Contains("Record") &&
                    PluginExecutionContext.InputParameters["Record"] is EntityReference)
                {
                    return (EntityReference)PluginExecutionContext.InputParameters["Assignee"];
                }

                return null;
            }
        }

        internal EntityReference EntityMoniker
        {
            get
            {
                if (PluginExecutionContext.InputParameters.Contains("EntityMoniker") &&
                    PluginExecutionContext.InputParameters["EntityMoniker"] is EntityReference)
                {
                    return (EntityReference)PluginExecutionContext.InputParameters["EntityMoniker"];
                }

                return null;
            }
        }

        internal QueryExpression Query
        {
            get
            {
                if (PluginExecutionContext.InputParameters.Contains("Query") &&
                    PluginExecutionContext.InputParameters["Query"] is QueryExpression)
                {
                    return (QueryExpression)PluginExecutionContext.InputParameters["Query"];
                }

                return null;
            }
        }

        internal Entity BusinessEntity
        {
            get
            {
                if (PluginExecutionContext.OutputParameters.Contains("BusinessEntity") &&
                    PluginExecutionContext.OutputParameters["BusinessEntity"] is Entity)
                {
                    return (Entity)PluginExecutionContext.OutputParameters["BusinessEntity"];
                }

                return null;
            }
        }

        internal EntityCollection BusinessEntityCollection
        {
            get
            {
                if (PluginExecutionContext.OutputParameters.Contains("BusinessEntityCollection") &&
                    PluginExecutionContext.OutputParameters["BusinessEntityCollection"] is EntityCollection)
                {
                    return (EntityCollection)PluginExecutionContext.OutputParameters["BusinessEntityCollection"];
                }

                return null;
            }
        }

        internal Entity OpportunityClose
        {
            get
            {
                if (PluginExecutionContext.InputParameters.Contains("OpportunityClose") &&
                    PluginExecutionContext.InputParameters["OpportunityClose"] is Entity)
                {
                    return (Entity)PluginExecutionContext.InputParameters["OpportunityClose"];
                }

                return null;
            }
        }

        //For Win/Lose messages
        internal EntityReference OpportunityReference
        {
            get
            {
                if (OpportunityClose.Contains("opportunityid") && OpportunityClose.Attributes["opportunityid"] != null)
                {
                    return (EntityReference)OpportunityClose.Attributes["opportunityid"];
                }
                return null;
            }
        }

        internal Guid? SubordinateId
        {
            get
            {
                if (PluginExecutionContext.InputParameters.Contains("SubordinateId") &&
                    PluginExecutionContext.InputParameters["SubordinateId"] is Guid)
                {
                    return (Guid)PluginExecutionContext.InputParameters["SubordinateId"];
                }
                return null;
            }
        }

        internal Guid? TeamTemplateId
        {
            get
            {
                if (PluginExecutionContext.InputParameters.Contains("TeamTemplateId") &&
                    PluginExecutionContext.InputParameters["TeamTemplateId"] is Guid)
                {
                    return (Guid)PluginExecutionContext.InputParameters["TeamTemplateId"];
                }
                return null;
            }
        }

        internal Guid? SystemUserId
        {
            get
            {
                if (PluginExecutionContext.InputParameters.Contains("SystemUserId") &&
                    PluginExecutionContext.InputParameters["SystemUserId"] is Guid)
                {
                    return (Guid)PluginExecutionContext.InputParameters["SystemUserId"];
                }
                return null;
            }
        }


        /// <summary>
        /// For AddMember and AddListMembers
        /// </summary>
        internal Guid? ListId
        {
            get
            {
                if (PluginExecutionContext.InputParameters.Contains("ListId") &&
                    PluginExecutionContext.InputParameters["ListId"] is Guid)
                {
                    return (Guid)PluginExecutionContext.InputParameters["ListId"];
                }
                return null;
            }
        }

        /// <summary>
        /// For AddMember message
        /// </summary>
        internal Guid? EntityId
        {
            get
            {
                if (PluginExecutionContext.InputParameters.Contains("EntityId") &&
                    PluginExecutionContext.InputParameters["EntityId"] is Guid)
                {
                    return (Guid)PluginExecutionContext.InputParameters["EntityId"];
                }
                return null;
            }
        }

        /// <summary>
        /// For AddListMembers message
        /// </summary>
        internal Guid[] MemberIds
        {
            get
            {
                if (PluginExecutionContext.InputParameters.Contains("MemberIds") &&
                    PluginExecutionContext.InputParameters["MemberIds"] is Guid[])
                {
                    return (Guid[])PluginExecutionContext.InputParameters["MemberIds"];
                }
                return null;
            }
        }


        /// <summary>
        /// Can be casted into IncidentResolution
        /// </summary>
        internal Entity IncidentResolution
        {
            get
            {
                if (PluginExecutionContext.InputParameters.Contains("IncidentResolution") &&
                    PluginExecutionContext.InputParameters["IncidentResolution"] is Entity)
                {
                    return (Entity)PluginExecutionContext.InputParameters["IncidentResolution"];
                }

                return null;
            }
        }

        internal OptionSetValue State
        {
            get
            {
                if (PluginExecutionContext.InputParameters.Contains("State") &&
                    PluginExecutionContext.InputParameters["State"] is OptionSetValue)
                {
                    return (OptionSetValue)PluginExecutionContext.InputParameters["State"];
                }

                return null;
            }
        }

        internal OptionSetValue Status
        {
            get
            {
                if (PluginExecutionContext.InputParameters.Contains("Status") &&
                    PluginExecutionContext.InputParameters["Status"] is OptionSetValue)
                {
                    return (OptionSetValue)PluginExecutionContext.InputParameters["Status"];
                }

                return null;
            }
        }


        /// <summary>
        /// Used in Associate / Disacociate Messages
        /// </summary>
        internal Relationship Relationship
        {
            get
            {
                if (PluginExecutionContext.InputParameters.Contains("Relationship") &&
                    PluginExecutionContext.InputParameters["Relationship"] is Relationship)
                {
                    return (Relationship)PluginExecutionContext.InputParameters["Relationship"];
                }

                return null;
            }
        }

        /// <summary>
        /// Used in Associate / Disacociate Messages
        /// </summary>
        internal EntityReferenceCollection RelatedEntities
        {
            get
            {
                if (PluginExecutionContext.InputParameters.Contains("RelatedEntities") &&
                    PluginExecutionContext.InputParameters["RelatedEntities"] is EntityReferenceCollection)
                {
                    return (EntityReferenceCollection)PluginExecutionContext.InputParameters["RelatedEntities"];
                }

                return null;
            }
        }

        internal Entity Target
        {
            get
            {
                if (PluginExecutionContext.InputParameters.Contains("Target") &&
                    PluginExecutionContext.InputParameters["Target"] is Entity)
                {
                    return (Entity)PluginExecutionContext.InputParameters["Target"];
                }

                if (PluginExecutionContext.InputParameters.Contains("Target") && PluginExecutionContext.InputParameters["Target"] is EntityReference)
                {

                    var targetOnAssign = (EntityReference)PluginExecutionContext.InputParameters["Target"];
                    return new Entity(targetOnAssign.LogicalName)
                    {
                        Id = targetOnAssign.Id
                    };
                }
                return null;
            }
        }
        #endregion InputParameters/OutputParameters



        #region Target and Images

        /// <summary>
        /// Get the first preImage
        /// </summary>
        internal Entity PreImage
            => PluginExecutionContext.PreEntityImages != null && PluginExecutionContext.PreEntityImages.Count >= 1 ?
                PluginExecutionContext.PreEntityImages.FirstOrDefault().Value : null;

        /// <summary>
        /// Get the first postImage
        /// </summary>
        internal Entity PostImage
            => PluginExecutionContext.PostEntityImages != null && PluginExecutionContext.PostEntityImages.Count >= 1 ?
                PluginExecutionContext.PostEntityImages.FirstOrDefault().Value : null;

        /// <summary>
        /// DEPRECATE - Adds the target values to the existing preImage
        /// </summary>
        /// <param name="target"></param>
        /// <param name="preImage"></param>
        /// <returns></returns>
        [Obsolete("Use Actual method instead.")]
        internal Entity Merge(Entity target, Entity preImage)
        {
            if (preImage == null && target == null) return null;
            if (preImage == null)
                return target;
            if (target == null)
                return PreImage;

            var resultat = preImage;

            foreach (var a in target.Attributes)
            {
                if (resultat.Attributes.Contains(a.Key))
                {
                    resultat.Attributes[a.Key] = a.Value;
                }
                else
                {
                    resultat.Attributes.Add(a.Key, a.Value);
                }
            }

            return resultat;
        }

        /// <summary>
        /// Creates a new entity. Applies preimage (if exists), then target, then postimage (if exists)
        /// The resulting entity contains an actual image of the record (given that all the fields were registered correctly) 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="preimage"></param>
        /// <param name="postimage"></param>
        /// <returns></returns>
        internal Entity Actual(Entity preimage, Entity target, Entity postimage)
        {

            var actual = new Entity(PluginExecutionContext.PrimaryEntityName,PluginExecutionContext.PrimaryEntityId);

            if (preimage != null)
            {
                Apply(actual, preimage);
            }

            if (target != null)
            {
                Apply(actual, target);
            }

            if (postimage != null)
            {
                Apply(actual, postimage);
            }

            return actual;
        }

        /// <summary>
        /// Applies the entity (to apply) ovec the current entity
        /// </summary>
        /// <param name="current"></param>
        /// <param name="toapply"></param>
        private static void Apply(Entity current, Entity toapply)
        {
            foreach (var attr in toapply.Attributes)
            {
                if (current.Attributes.Contains(attr.Key))
                {
                    current.Attributes[attr.Key] = attr.Value;
                }
                else
                {
                    current.Attributes.Add(attr.Key, attr.Value);
                }
            }
        }

        /// <summary>
        /// Creates a new entity. Applies preimage (if exists), then target, then postimage (if exists)
        /// The resulting entity contains an actual image of the record (given that all the fields were registered correctly) 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal T Actual<T>() where T : Entity
        {
            return Actual(PreImage, Target, PostImage)?.ToEntity<T>();
        }



        /// <summary>
        /// Adds the target values to the existing preImage. Changes won't go to step 30. Use the Target for that.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Obsolete("Use Actual method instead.")]
        internal T Merge<T>() where T : Entity
        {
            return Merge(Target, PreImage)?.ToEntity<T>();
        }

        [Obsolete("Use Actual method instead.")]
        internal T MergePost<T>() where T : Entity
        {
            return Merge(Target, PostImage)?.ToEntity<T>();
        }

        /// <summary>
        /// To use when the plugin is triggered on create, update and SetState
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Obsolete("Use Actual method instead.")]
        internal T MergeOrEntityMoniker<T>() where T : Entity, new()
        {
            return Merge<T>() ?? new T() { Id = EntityMoniker.Id };
        }

        #endregion


        



        
    }

    private Collection<Tuple<int, string, string, Action<LocalPluginContext>>> _registeredEvents;

    /// <summary>
    /// Gets the List of events that the plug-in should fire for. Each List
    /// Item is a <see cref="Tuple"/> containing the Pipeline Stage, Message and (optionally) the Primary Entity. 
    /// In addition, the fourth parameter provide the delegate to invoke on a matching registration.
    /// </summary>
    protected Collection<Tuple<int, string, string, Action<LocalPluginContext>>> RegisteredEvents
        => _registeredEvents ??
           (_registeredEvents = new Collection<Tuple<int, string, string, Action<LocalPluginContext>>>());

    /// <summary>
    /// Gets the name of the instantiated class.
    /// </summary>
    /// <value>The name of the instantiated class.</value>
    protected string DerivedClassName => GetType().ToString();

    
    
    
    

    /// <summary>
    /// Executes the plug-in.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <remarks>
    /// For improved performance, Microsoft Dynamics CRM caches plug-in instances. 
    /// The plug-in's Execute method should be written to be stateless as the constructor 
    /// is not called for every invocation of the plug-in. Also, multiple system threads 
    /// could execute the plug-in at the same time. All per invocation state information 
    /// is stored in the context. This means that you should not use global variables in plug-ins.
    /// </remarks>
    public void Execute(IServiceProvider serviceProvider)
    {
        if (serviceProvider == null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        // Construct the Local plug-in context.
        var localcontext = new LocalPluginContext(serviceProvider);

        localcontext.Trace($"Entered {DerivedClassName}.Execute()");


        try
        {
            // Iterate over all of the expected registered events to ensure that the plugin
            // has been invoked by an expected event
            // For any given plug-in event at an instance in time, we would expect at most 1 result to match.
            var entityAction = RegisteredEvents
                                    .Where(r => r.Item1 == localcontext.PluginExecutionContext.Stage &&
                                                            r.Item2 == localcontext.PluginExecutionContext.MessageName &&
                                                            (string.IsNullOrWhiteSpace(r.Item3) ||
                                                             r.Item3 == localcontext.PluginExecutionContext.PrimaryEntityName))
                                    .Select(r => r.Item4)
                                    .FirstOrDefault();

            if (entityAction == null) return;

            localcontext.Trace(
                $"{DerivedClassName} is firing for Entity: {localcontext.PluginExecutionContext.PrimaryEntityName}, Message: {localcontext.PluginExecutionContext.MessageName}");

            entityAction.Invoke(localcontext);

            // now exit - if the derived plug-in has incorrectly registered overlapping event registrations,
            // guard against multiple executions.
            return;
        }
        catch (Exception ex)
        {
            /*-------------------------------------------------------------------------------------------------- 
            Si c’est vous qui avez lancé une InvalidPluginExecutionException, alors ce n’est pas imprévue et on 
            ne fait que la relancer à CRM. Si vous voulez lancer une exception qui doit être formatée en 
            erreur, lancer une System.Exception à la place. 
            --------------------------------------------------------------------------------------------------*/
            if (ex is InvalidPluginExecutionException)
            {
                throw;
            }
            else
            {
                /*-------------------------------------------------------------------------------------------------- 
                Sinon, c’est une exception imprévue. Alors on formate un message. 
                --------------------------------------------------------------------------------------------------*/
                var msg = new StringWriter();
                msg.WriteLine($"An unexpected error occurred: {ex.Message}");    
                
                /*-------------------------------------------------------------------------------------------------- 
                Ce qui nous intéresse vraiment au-delà du message de l’exception, ce sont les particularités de 
                certaines exceptions, par exemple l’erreur SOAP suite à un appel de SDK à CRM (maintenant pris en 
                charge via OrganizationServiceFault). L’exception reçue est peut-être du type recherché mais il 
                est aussi probable qu’elle soit un niveau plus bas dans la chaine (InnerException). 
                --------------------------------------------------------------------------------------------------*/
                FaultException<OrganizationServiceFault> organizationServiceFault = null;
                if (ex is FaultException<OrganizationServiceFault>)
                    organizationServiceFault = (FaultException<OrganizationServiceFault>)ex;
                if (organizationServiceFault == null && ex.InnerException is FaultException<OrganizationServiceFault>)
                    organizationServiceFault = (FaultException<OrganizationServiceFault>)ex.InnerException;

                if (organizationServiceFault != null)
                {
                    /*-------------------------------------------------------------------------------------------------- 
                    Formater la OrganizationServiceFault trouvée 
                    --------------------------------------------------------------------------------------------------*/
                    msg.WriteLine("OrganizationServiceFault details:");
                    msg.WriteLine($"Fault Timestamp: {organizationServiceFault.Detail.Timestamp}");
                    msg.WriteLine($"Fault Error Code: {organizationServiceFault.Detail.ErrorCode}");
                    msg.WriteLine($"Fault Message: {organizationServiceFault.Detail.Message}");

                    if (organizationServiceFault.Detail.InnerFault != null
                        && organizationServiceFault.Message != organizationServiceFault.Detail.InnerFault.Message)
                    {
                        msg.WriteLine($"Inner Fault Timestamp: {organizationServiceFault.Detail.InnerFault.Timestamp}");
                        msg.WriteLine($"Inner Fault Error Code: {organizationServiceFault.Detail.InnerFault.ErrorCode}");
                        msg.WriteLine($"Inner Fault Message: {organizationServiceFault.Detail.InnerFault.Message}");
                    }
                }

                /*-------------------------------------------------------------------------------------------------- 
                On ajoute le stack trac à la fin car c’est lui qui prend le plus de place 
                --------------------------------------------------------------------------------------------------*/
                msg.WriteLine($"Stack Trace: {ex.StackTrace}");

                /*-------------------------------------------------------------------------------------------------- 
                On renvoie le tous à CRM via une InvalidPluginExecutionException au statut Failed 
                --------------------------------------------------------------------------------------------------*/
                localcontext.Trace(msg.ToString());

                if (localcontext.Target != null)
                {
                    msg.WriteLine("--Target--");

                    msg.WriteLine(Crunch(localcontext.Target));

                    msg.WriteLine("");
                }

                if (localcontext.PreImage != null)
                {
                    msg.WriteLine("--PreImage--");

                    // Ajouter le contenu du preimage
                    msg.WriteLine(Crunch(localcontext.PreImage));

                    msg.WriteLine("");
                }

                if (localcontext.PostImage != null)
                {
                    msg.WriteLine("--PostImage--");

                    // Ajouter le contenu du preimage
                    msg.WriteLine(Crunch(localcontext.PostImage));

                    msg.WriteLine("");
                }

                throw new InvalidPluginExecutionException(OperationStatus.Failed, msg.ToString());
            }
        }
        finally
        {
            localcontext.Trace( $"Exiting {DerivedClassName}.Execute()");
        }
    }

    private static StringBuilder Crunch(Entity entity)
    {
        var msg = new StringBuilder();

        if (entity == null)
            return msg;

        // Ajouter le contenu du target
        foreach (var attrib in entity.Attributes)
        {
            string value;

            if (attrib.Value == null)
                value = "null";
            else if (attrib.Value is OptionSetValue)
                value = ((OptionSetValue)attrib.Value).Value.ToString();
            else if (attrib.Value is EntityReference)
                value =
                    $"{((EntityReference)attrib.Value).LogicalName}-{((EntityReference)attrib.Value).Id}-{((EntityReference)attrib.Value).Name}";
            else if (attrib.Value is Entity)
                value =
                    $"{((Entity)attrib.Value).LogicalName}-{((Entity)attrib.Value).Id}-{((Entity)attrib.Value).ToEntityReference().Name}";
            else
                value = attrib.Value.ToString();

            msg.AppendLine($"{attrib.Key}: {value}");
        }

        return msg;
    }

    protected void RegisterEvent<T>(EventOperation eventOperation, ExecutionStage executionStage, Action<LocalPluginContext> action) where T : Entity 
        => RegisteredEvents.Add(
            new Tuple<int, string, string, Action<LocalPluginContext>>(
                (int)executionStage,
                eventOperation.ToString(),
                Activator.CreateInstance<T>().LogicalName,
                action));

    //usefull for custom actions
    protected void RegisterEvent<T>(string eventOperationName, ExecutionStage executionStage, Action<LocalPluginContext> action) where T : Entity
    => RegisteredEvents.Add(
        new Tuple<int, string, string, Action<LocalPluginContext>>(
            (int)executionStage,
            eventOperationName,
            Activator.CreateInstance<T>().LogicalName,
            action));


    //Used for late bound 
    protected void RegisterEvent(string entityLogicalName, EventOperation eventOperation, ExecutionStage executionStage, Action<LocalPluginContext> action) 
        => RegisteredEvents.Add(
            new Tuple<int, string, string, Action<LocalPluginContext>>(
                (int)executionStage,
                eventOperation.ToString(),
                entityLogicalName,
                action));

    protected void RegisterEventOnAllEntities(EventOperation eventOperation, ExecutionStage executionStage, Action<LocalPluginContext> action)
        => RegisteredEvents.Add(
            new Tuple<int, string, string, Action<LocalPluginContext>>(
                (int)executionStage,
                eventOperation.ToString(),
                null,
                action));

    protected void RegisterEventOnAllEntities(string eventOperationName, ExecutionStage executionStage, Action<LocalPluginContext> action)
    => RegisteredEvents.Add(
        new Tuple<int, string, string, Action<LocalPluginContext>>(
            (int)executionStage,
            eventOperationName,
            null,
            action));

    protected void RegisterCustomApi(string eventOperationName, Action<LocalPluginContext> action)
    => RegisteredEvents.Add(
        new Tuple<int, string, string, Action<LocalPluginContext>>(
            (int)ExecutionStage.MainOperation,  //custom Api are registered on Main operation (30)
            eventOperationName,
            null,
            action));

    protected void RegisterCustomApi<T>(string eventOperationName, Action<LocalPluginContext> action) where T: Entity
    => RegisteredEvents.Add(
        new Tuple<int, string, string, Action<LocalPluginContext>>(
            (int)ExecutionStage.MainOperation,  //custom Api are registered on Main operation (30)
            eventOperationName,
            null,
            action));

    public enum Locale
    {
        English = 1033,
        French = 1036
    }
    public enum ExecutionStage
    { 
        PreValidation = 10, 
        PreOperation = 20,
        MainOperation = 30,  //with Custom API, you need to register on MainOperation
        PostOperation = 40, 
    }

    // EventOperation based on CRM 2016
    public enum EventOperation
    {
        AddItem,
        AddListMembers,
        AddMember,
        AddMembers,
        AddPrincipalToQueue,
        AddPrivileges,
        AddProductToKit,
        AddRecurrence,
        AddToQueue,
        AddUserToRecordTeam,
        ApplyRecordCreationAndUpdateRule,
        Assign,
        AssignUserRoles,
        Associate,
        BackgroundSend,
        Book,
        CalculatePrice,
        Cancel,
        CheckIncoming,
        CheckPromote,
        Clone,
        CloneProduct,
        Close,
        CopyDynamicListToStatic,
        CopySystemForm,
        Create,
        CreateException,
        CreateInstance,
        CreateKnowledgeArticleTranslation,
        CreateKnowledgeArticleVersion,
        Delete,
        DeleteOpenInstances,
        DeliverIncoming,
        DeliverPromote,
        DetachFromQueue,
        Disassociate,
        Execute,
        ExecuteById,
        Export,
        ExportAll,
        ExportCompressed,
        ExportCompressedAll,
        GenerateSocialProfile,
        GetDefaultPriceLevel,
        GrantAccess,
        Handle,
        Import,
        ImportAll,
        ImportCompressedAll,
        ImportCompressedWithProgress,
        ImportWithProgress,
        LockInvoicePricing,
        LockSalesOrderPricing,
        Lose,
        Merge,
        ModifyAccess,
        PickFromQueue,
        Publish,
        PublishAll,
        PublishTheme,
        QualifyLead,
        Recalculate,
        ReleaseToQueue,
        RemoveFromQueue,
        RemoveItem,
        RemoveMember,
        RemoveMembers,
        RemovePrivilege,
        RemoveProductFromKit,
        RemoveRelated,
        RemoveUserFromRecordTeam,
        RemoveUserRoles,
        ReplacePrivileges,
        Reschedule,
        Retrieve,
        RetrieveExchangeRate,
        RetrieveFilteredForms,
        RetrieveMultiple,
        RetrievePersonalWall,
        RetrievePrincipalAccess,
        RetrieveRecordWall,
        RetrieveSharedPrincipalsAndAccess,
        RetrieveUnpublished,
        RetrieveUnpublishedMultiple,
        RetrieveUserQueues,
        RevokeAccess,
        Route,
        RouteTo,
        Send,
        SendFromTemplate,
        SetLocLabels,
        SetRelated,
        SetState,
        SetStateDynamicEntity,
        TriggerServiceEndpointCheck,
        UnlockInvoicePricing,
        UnlockSalesOrderPricing,
        Update,
        ValidateRecurrenceRule,
        Win
    }



}
