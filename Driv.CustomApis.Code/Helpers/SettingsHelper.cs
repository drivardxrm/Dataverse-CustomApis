using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using System.Linq;

namespace Driv.CustomApis.Helpers
{
    public static class SettingsHelper
    {
        public static SettingDefinition GetSettingDefinitionFor(this XrmContext xrmcontext, string settingname)
            => xrmcontext.SettingDefinitionSet.SingleOrDefault(s => s.UniqueName == settingname);

        public static SettingDetail RetrieveSetting(this IOrganizationService service, string settingname, string appname) 
        {
            // USING OOB RetrieveSetting Function
            // https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/retrievesetting?view=dynamics-ce-odata-9

            var request = new OrganizationRequest("RetrieveSetting");
            request.Parameters.Add("SettingName", settingname);
            request.Parameters.Add("AppUniqueName", appname);
            
            var response = service.Execute(request);


            return response.Results["SettingDetail"] as SettingDetail;
        }

    }
}
