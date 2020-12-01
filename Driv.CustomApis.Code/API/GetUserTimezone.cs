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
            RegisterEventOnAllEntities("driv_GetUserTimezone", ExecutionStage.MainOperation, Execute);
        }

        public void Execute(LocalPluginContext localcontext)
        {

            var service = localcontext.OrganizationService;
            var inputparameters = localcontext.PluginExecutionContext.InputParameters;
            var outputparameters = localcontext.PluginExecutionContext.OutputParameters;
            var user = localcontext.Target.ToEntity<SystemUser>();

            var xrmcontext = new XrmContext(service);

            localcontext.Trace($"TARGET : {user.Id}\n");
            inputparameters.ToList().ForEach(
                i => localcontext.Trace($"INPUT PARAMETER : {i}\n")
            );



            var timezonedef = xrmcontext.GetTimezoneDefinitionFor(user);



            outputparameters["TimezoneCode"] = timezonedef.TimeZoneCode;
            outputparameters["TimezoneName"] = timezonedef.StandardName;

            outputparameters.ToList().ForEach(
                i => localcontext.Trace($"OUTPUT PARAMETER : {i}\n")
            );
        }

    }
}
