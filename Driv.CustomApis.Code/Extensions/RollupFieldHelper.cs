using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace XrmVision.Extensions.Extensions
{
    public static class RollupFieldHelper
    {

        public static IEnumerable<AttributeMetadata> GetRollupFields(this IOrganizationService service,
            string entityName)
        {
            var retrieveEntityResponse = (RetrieveEntityResponse) service.Execute(new RetrieveEntityRequest()
            {
                EntityFilters = EntityFilters.Attributes,
                LogicalName = entityName,
                RetrieveAsIfPublished = false
            });

            var entityMetadata = retrieveEntityResponse.EntityMetadata;

            return entityMetadata.Attributes.Where(a => a.SourceType == 2 &&
                                                        (a.GetType() != typeof(MoneyAttributeMetadata) ||
                                                         a is MoneyAttributeMetadata &&
                                                         ((MoneyAttributeMetadata) a).CalculationOf == null));

        }

        public static IEnumerable<AttributeMetadata> GetRollupFields<T>(this IOrganizationService service)
            where T : Entity
        {
            return service.GetRollupFields(Activator.CreateInstance<T>().LogicalName);
        }

        public static void CalculateRollup(this IOrganizationService service, EntityReference target, string fieldname)
        {
            service.Execute(new CalculateRollupFieldRequest()
            {
                FieldName = fieldname,
                Target = target
            });
        }

        public static void CalculateAllRollups(this IOrganizationService service, EntityReference target)
        {
            service.GetRollupFields(target.LogicalName).ToList().ForEach(a =>
            {
                service.CalculateRollup(target, a.LogicalName);
            });
        }

    }

}