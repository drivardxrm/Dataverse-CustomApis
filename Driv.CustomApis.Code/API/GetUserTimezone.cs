using Driv.CustomApis.Helpers;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driv.CustomApis.API
{
    public class GetUserTimezone : PluginBase
    {
        public GetUserTimezone()
        {
            RegisterCustomApi<SystemUser>("driv_GetUserTimezone", Execute);
        }

        public void Execute(LocalPluginContext localcontext)
        {

            var service = localcontext.OrganizationService;
            
            var outputparameters = localcontext.PluginExecutionContext.OutputParameters;
            var user = localcontext.Target.ToEntity<SystemUser>();

            var xrmcontext = new XrmContext(service);


            var timezonedef = xrmcontext.GetTimezoneDefinitionFor(user);

            outputparameters["TimezoneCode"] = timezonedef.TimeZoneCode;
            outputparameters["TimezoneName"] = timezonedef.StandardName;

            
        }

    }
}
