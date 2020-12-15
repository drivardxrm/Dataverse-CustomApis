using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;


namespace XrmVision.Extensions.Extensions
{
    public static class OptionSetHelper
    {
        public static string GetOptionsetText(this IOrganizationService service, string entityName, string attributeName, int optionSetValue)
        {

            var attmetadata = service.GetAttributeMetadata(entityName, attributeName) as EnumAttributeMetadata;

            return attmetadata?.OptionSet?.Options?.Where(x => x.Value == optionSetValue).FirstOrDefault()?
                                                .Label?.UserLocalizedLabel?.Label;

        }

        public static string GetOptionSetText(this IOrganizationService service, string entityName, string attributeName, int optionSetValue, int langcode)
        {
            var attmetadata = service.GetAttributeMetadata(entityName, attributeName) as EnumAttributeMetadata;



            return attmetadata?.OptionSet?.Options.Where(x => x.Value == optionSetValue).FirstOrDefault()?
                                                .Label?.LocalizedLabels?.Where(l => l.LanguageCode == langcode).FirstOrDefault()?
                                                .Label;
        }



        


        public static int GetOptionsetInt(this IOrganizationService service, string entityName, string attributeName, string optionSetText)
        {
            try
            {
                var picklistMetadata = service.GetPickListMetadata(entityName, attributeName);

                if (picklistMetadata == null)
                {
                    return -1;
                }

                var options = picklistMetadata.OptionSet;
                IList<OptionMetadata> optionsList = options.Options
                    .Where(o => o.Label.UserLocalizedLabel.Label.ToLower() == optionSetText.ToLower()).ToList();
                var optionsetvalue = optionsList.First()?.Value.Value;
                return optionsetvalue ?? -1 ;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static OptionSetValue GetOptionsetValue(this IOrganizationService service, string entityName, string attributeName, string optionSetText)
        {
            return new OptionSetValue(service.GetOptionsetInt(entityName, attributeName, optionSetText));
        }

        private static PicklistAttributeMetadata GetPickListMetadata(this IOrganizationService service, string entityName, string attributeName)
        {
            var retrieveDetails = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.All,
                LogicalName = entityName
            };
            var retrieveEntityResponseObj = (RetrieveEntityResponse)service.Execute(retrieveDetails);
            var metadata = retrieveEntityResponseObj.EntityMetadata;
            var picklistMetadata = metadata.Attributes.FirstOrDefault(attribute =>
                    string.Equals(attribute.LogicalName, attributeName, StringComparison.OrdinalIgnoreCase)) as
                PicklistAttributeMetadata;

            return picklistMetadata;
        }

    }
}
