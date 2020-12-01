using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;


namespace XrmVision.Extensions.Extensions
{
    public static class EntityHelper
    {

        public static T FromId<T>(this Guid id) where T : Entity
        {
            var entity = Activator.CreateInstance<T>();
            entity.Id = id;

            return entity;
        }

        public static T FromRef<T>(this EntityReference entityref) where T : Entity
        {
            var entity = Activator.CreateInstance<T>();
            entity.Id = entityref.Id;

            return entity;
        }
        public static T Get<T>(this OrganizationServiceContext context, Guid id) where T : Entity
            => context.CreateQuery<T>().FirstOrDefault(e => e.Id == id);

        public static T Get<T>(this OrganizationServiceContext context, EntityReference reference) where T : Entity
            => reference != null ? context.Get<T>(reference.Id) : null;

        public static IEnumerable<T> Get<T>(this OrganizationServiceContext context, IEnumerable<Guid> guids)
            where T : Entity
            => guids.Select(context.Get<T>);


        public static IEnumerable<T> Get<T>(this OrganizationServiceContext context,
            IEnumerable<EntityReference> references) where T : Entity
            => references.Select(context.Get<T>);


        public static bool Is<T>(this EntityReference entityref) where T : Entity
            => entityref.LogicalName == Activator.CreateInstance<T>().LogicalName;

        public static EntityReference GetEntityReference<T>(Guid guid) where T : Entity
            => new EntityReference(logicalName: Activator.CreateInstance<T>().LogicalName, id: guid);



        public static EntityReferenceCollection ToEntityReferenceCollection(this IQueryable<Entity> entities)
            => entities.ToList().ToEntityReferenceCollection();

        public static EntityReferenceCollection ToEntityReferenceCollection(this IEnumerable<Entity> entities)
            => entities.ToList().ToEntityReferenceCollection();

        public static EntityReferenceCollection ToEntityReferenceCollection(this IList<Entity> entities)
        {
            var entityRefCol = new EntityReferenceCollection();
            foreach (var entity in entities)
            {
                entityRefCol.Add(entity.ToEntityReference());
            }
            return entityRefCol;
        }

        public static EntityReferenceCollection ToEntityReferenceCollection(this EntityReference entityref)
            => new EntityReferenceCollection {entityref};

        public static Guid Upsert(this IOrganizationService service, Entity entity)
        {
            Guid guid;
            if (entity.Id == Guid.Empty)
            {
                guid = service.Create(entity);
            }
            else
            {
                service.Update(entity);
                guid =  entity.Id;
            }
            return guid;

        }


        public static void Associate(this IOrganizationService service, EntityReference target, string relationshipname,
            EntityReferenceCollection source)
        {
            service.Associate(target.LogicalName,
                target.Id,
                new Relationship(relationshipname),
                source);
        }

        public static void Associate(this IOrganizationService service, EntityReference target, string relationshipname,
            EntityReference source)
        {
            service.Associate(target.LogicalName,
                target.Id,
                new Relationship(relationshipname),
                source.ToEntityReferenceCollection());
        }

        public static void Disassociate(this IOrganizationService service, EntityReference target,
            string relationshipname, EntityReferenceCollection source)
        {
            service.Disassociate(target.LogicalName,
                target.Id,
                new Relationship(relationshipname),
                source);
        }

        public static void Disassociate(this IOrganizationService service, EntityReference target,
            string relationshipname, EntityReference source)
        {
            service.Disassociate(target.LogicalName,
                target.Id,
                new Relationship(relationshipname),
                source.ToEntityReferenceCollection());
        }



        public static void DisassociateRelated<T>(this IOrganizationService service,
            EntityReference primaryEntityReference, string relationshipname) where T : Entity
        {
            var associatedentities = service.GetRelatedEntities<T>(primaryEntityReference, relationshipname);
            service.Disassociate(primaryEntityReference, relationshipname,
                associatedentities.ToEntityReferenceCollection());
        }

        public static void Delete(this IOrganizationService service, EntityReference entityref)
        {
            service.Delete(entityref.LogicalName, entityref.Id);
        }

        public static void Delete(this IOrganizationService service, Entity entity)
        {
            service.Delete(entity.ToEntityReference());
        }

        /// <summary>
        /// Find the Logical Name from the entity type code - this needs a reference to the Organization Service to look up metadata
        /// </summary>
        /// <param name="service"></param>
        /// <param name="entityTypeCode"></param>
        /// <returns></returns>
        public static string GetEntityLogicalName(this IOrganizationService service, int entityTypeCode)
        {
            var entityFilter = new MetadataFilterExpression(LogicalOperator.And);
            entityFilter.Conditions.Add(new MetadataConditionExpression("ObjectTypeCode ",
                MetadataConditionOperator.Equals, entityTypeCode));

            var propertyExpression = new MetadataPropertiesExpression {AllProperties = false};
            propertyExpression.PropertyNames.Add("LogicalName");

            var entityQueryExpression = new EntityQueryExpression
            {
                Criteria = entityFilter,
                Properties = propertyExpression
            };

            var retrieveMetadataChangesRequest = new RetrieveMetadataChangesRequest
            {
                Query = entityQueryExpression
            };

            var response = (RetrieveMetadataChangesResponse) service.Execute(retrieveMetadataChangesRequest);

            return response.EntityMetadata.Count == 1 ? response.EntityMetadata[0].LogicalName : null;
        }

        public static DataCollection<Entity> GetRelatedEntities(this IOrganizationService service,
            string primaryEntityName, Guid primaryEntityId, string relationshipName, string targetEntityName)
        {
            //the related entity we are going to retrieve
            var query = new QueryExpression
            {
                EntityName = targetEntityName,
                ColumnSet = new ColumnSet()
            };
            query.ColumnSet.AllColumns = true;

            //the relationship that links the primary to the target
            var relationship = new Relationship(relationshipName) {PrimaryEntityRole = EntityRole.Referenced};
            //important if the relationship is self-referencing

            //the query collection which forms the request
            var relatedEntity = new RelationshipQueryCollection {{relationship, query}};

            //the request to get the primary entity with the related records
            var request = new RetrieveRequest
            {
                RelatedEntitiesQuery = relatedEntity,
                ColumnSet = new ColumnSet(),
                Target = new EntityReference(primaryEntityName, primaryEntityId)
            };

            var response = (RetrieveResponse) service.Execute(request);

            //query the returned collection for the target entity ids
            return response.Entity.RelatedEntities[relationship].Entities;
        }

        public static IEnumerable<Guid> GetRelatedEntitiesId(this IOrganizationService service,
            string primaryEntityName, Guid primaryEntityId, string relationshipName, string targetEntityName)
            => service.GetRelatedEntities(primaryEntityName, primaryEntityId, relationshipName, targetEntityName)
                .Select(e => e.Id);

        public static IEnumerable<Guid> GetRelatedEntitiesId(this IOrganizationService service,
            EntityReference primaryEntity,
            string relationshipName, string targetEntityName)
            => service.GetRelatedEntitiesId(primaryEntity.LogicalName, primaryEntity.Id, relationshipName,
                targetEntityName);

        public static IEnumerable<EntityReference> GetRelatedEntitiesRef(this IOrganizationService service,
            string primaryEntityName, Guid primaryEntityId, string relationshipName, string targetEntityName)
            => service.GetRelatedEntities(primaryEntityName, primaryEntityId, relationshipName, targetEntityName)
                .Select(e => e.ToEntityReference());

        public static IEnumerable<EntityReference> GetRelatedEntitiesRef(this IOrganizationService service,
            EntityReference primaryEntity,
            string relationshipName, string targetEntityName)
            => service.GetRelatedEntitiesRef(primaryEntity.LogicalName, primaryEntity.Id, relationshipName,
                targetEntityName);

        public static IEnumerable<T> GetRelatedEntities<T>(this IOrganizationService service, string primaryEntityName,
            Guid primaryEntityId, string relationshipName) where T : Entity
            => service.GetRelatedEntities(primaryEntityName, primaryEntityId, relationshipName,
                Activator.CreateInstance<T>().LogicalName).Select(e => e.ToEntity<T>());

        public static IEnumerable<T> GetRelatedEntities<T>(this IOrganizationService service,
            EntityReference primaryEntity,
            string relationshipName) where T : Entity
            => service.GetRelatedEntities<T>(primaryEntity.LogicalName, primaryEntity.Id, relationshipName);


        /// <summary>
        /// Clears the context and returns it. can be used as chaining method
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static OrganizationServiceContext Clear(this OrganizationServiceContext context)
        {
            context.ClearChanges();

            return context;
        }



    }
}

