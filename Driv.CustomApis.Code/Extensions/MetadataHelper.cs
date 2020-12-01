using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace XrmVision.Extensions.Extensions
{
    public static class MetadataHelper
    {
        public static EntityMetadata[] GetAllMetadata(this IOrganizationService service)
        {
            var retrieveAllEntityRequest = new RetrieveAllEntitiesRequest
            {
                RetrieveAsIfPublished = false,
                EntityFilters = EntityFilters.Attributes
            };
            var retrieveAllEntityResponse = (RetrieveAllEntitiesResponse) service.Execute(retrieveAllEntityRequest);

            return retrieveAllEntityResponse.EntityMetadata;
        }

        public static EntityMetadata GetMetadata(this IOrganizationService service, string entityname)
        {
            RetrieveEntityRequest entityRequest = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Attributes,
                LogicalName = entityname,
                RetrieveAsIfPublished = false
            };
            RetrieveEntityResponse entityResponse = (RetrieveEntityResponse) service.Execute(entityRequest);

            return entityResponse.EntityMetadata;
        }

        

        public static EntityMetadata GetMetadata<T>(this IOrganizationService service) where T : Entity
            => service.GetMetadata(Activator.CreateInstance<T>().LogicalName);


        public static AttributeMetadata GetAttributeMetadata(this IOrganizationService service, string entityName, string attributeName)
        {
            var attReq = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = attributeName,
                RetrieveAsIfPublished = true
            };



            var attResponse = (RetrieveAttributeResponse)service.Execute(attReq);
            return attResponse.AttributeMetadata;

        }

        public static string GetFieldDisplayName(this IOrganizationService service, string entityName, string attributeName)
        {
            var attmetadata = service.GetAttributeMetadata(entityName, attributeName);

            return attmetadata.DisplayName.UserLocalizedLabel.Label;
        }

        public static string GetFieldDisplayName(this IOrganizationService service, string entityName, string attributeName, int lang)
        {
            var attmetadata = service.GetAttributeMetadata(entityName, attributeName);

            return attmetadata.DisplayName.LocalizedLabels.Where(l => l.LanguageCode == lang).FirstOrDefault().Label;
        }

        public static int GetObjectTypeCode(this IOrganizationService service, string entityName)
        {
            var request = new RetrieveEntityRequest
            {
                LogicalName = entityName
            };
            var response = service.Execute(request) as RetrieveEntityResponse;
            return response.EntityMetadata.ObjectTypeCode.Value;
        }

        public static int GetObjectTypeCode<T>(this IOrganizationService service) where T : Entity 
            => service.GetObjectTypeCode(Activator.CreateInstance<T>().LogicalName);

    }

}