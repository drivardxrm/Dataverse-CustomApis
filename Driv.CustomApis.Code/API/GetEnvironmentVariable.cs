using Driv.CustomApis.Helpers;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driv.CustomApis.API
{
    public class GetEnvironmentVariable : PluginBase
    {
        public GetEnvironmentVariable()
        {
            RegisterEventOnAllEntities("driv_GetEnvironmentVariable", ExecutionStage.MainOperation, Execute);
        }

        public void Execute(LocalPluginContext localcontext)
        {

            var service = localcontext.OrganizationService;
            var inputparameters = localcontext.PluginExecutionContext.InputParameters;
            var outputparameters = localcontext.PluginExecutionContext.OutputParameters;

            var xrmcontext = new XrmContext(service);

            var key = inputparameters["Key"] as string;

            localcontext.Trace($"KEY:{key}\n");

            //default values
            outputparameters["Exists"] = false;
            outputparameters["ValueString"] = string.Empty;
            outputparameters["ValueBool"] = false;

            localcontext.Trace($"OUTPUT : DEFAULT\n");
            outputparameters.ToList().ForEach(o => localcontext.Trace($"--{o.Key}:{o.Value}\n"));
            //outputparameters["ValueNumber"] = decimal.Zero;
            //outputparameters["Type"] = ;


            var definition = xrmcontext.GetEnvironmentVariableDefinition(key);
            if (definition != null)
            {
                outputparameters["Exists"] = true;

                var overridenvalue = xrmcontext.GetEnvironmentVariableValue(definition);
                outputparameters["ValueString"] = overridenvalue != null ?
                                                            overridenvalue.Value :
                                                            definition.DefaultValue;

                //outputparameters["Type"] = definition.Type;

                if (definition.Type == EnvironmentVariableDefinition_Type.Boolean)
                {
                    outputparameters["ValueBool"] = overridenvalue != null ?
                                                            overridenvalue.Value == "yes" :
                                                            definition.DefaultValue == "yes";
                }

                //if (definition.Type == EnvironmentVariableDefinition_Type.Number)
                //{
                //    if (overridenvalue != null && decimal.TryParse(overridenvalue.Value, out decimal value) 
                //        || 
                //        decimal.TryParse(definition.DefaultValue, out value))
                //    {
                //        outputparameters["ValueNumber"] = value;
                //    }

                //}


            }





        }
        //    private void Execute(LocalPluginContext localcontext)
        //{
        //    var key = localcontext.PluginExecutionContext.InputParameters["Key"];

        //    localcontext.PluginExecutionContext.OutputParameters["Exists"] = true;

        //    localcontext.PluginExecutionContext.OutputParameters["Value"] = key;
        //}
    }
}
