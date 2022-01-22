using Driv.CustomApis.Helpers;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XrmVision.Extensions.Extensions;

namespace Driv.CustomApis.API
{
    public class GetEnvironmentVariable : PluginBase
    {
        public GetEnvironmentVariable()
        {
            RegisterCustomApi("driv_GetEnvironmentVariable", Execute);
        }

        public void Execute(LocalPluginContext localcontext)
        {

            var service = localcontext.OrganizationService;
            var inputparameters = localcontext.PluginExecutionContext.InputParameters;
            var outputparameters = localcontext.PluginExecutionContext.OutputParameters;

            var xrmcontext = new XrmContext(service);

            var key = inputparameters["Key"] as string;

           

            //default values
            outputparameters["Exists"] = false;
            outputparameters["ValueString"] = string.Empty;
            

            var definition = xrmcontext.GetEnvironmentVariableDefinition(key);
            if (definition != null)
            {
                outputparameters["Exists"] = true;

                var overridenvalue = xrmcontext.GetEnvironmentVariableValue(definition);
                outputparameters["ValueString"] = overridenvalue != null ?
                                                            overridenvalue.Value :
                                                            definition.DefaultValue;

 
                outputparameters["Type"] = new OptionSetValue((int)definition.Type);
                outputparameters["TypeName"] = service.GetOptionsetText(EnvironmentVariableDefinition.EntityLogicalName, "type", (int)definition.Type);

                switch (definition.Type) 
                {
                    case environmentvariabledefinition_type.Boolean:
                
                        outputparameters["ValueBool"] = overridenvalue != null ?
                                                            overridenvalue.Value == "yes" :
                                                            definition.DefaultValue == "yes";
                        break;

                    case environmentvariabledefinition_type.Number:
                        outputparameters["ValueDecimal"] = overridenvalue != null ?
                                                                 decimal.Parse(overridenvalue.Value):
                                                                 decimal.Parse(definition.DefaultValue);

                        break;
                    case environmentvariabledefinition_type.Secret:
                        var secretresponse = service.GetEnvironmentVariableSecretValue(key);
                        outputparameters["ValueSecret"] = secretresponse.EnvironmentVariableSecretValue;

                        break;

                } 
            }
        }
        
    }
}
