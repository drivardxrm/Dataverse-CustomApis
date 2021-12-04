using Driv.CustomApis.Helpers;
using Microsoft.Xrm.Sdk;
using XrmVision.Extensions.Extensions;

namespace Driv.CustomApis.API
{
    public class GetSetting : PluginBase
    {
        public GetSetting()
        {
            RegisterCustomApi("driv_GetSetting", Execute);
        }

        public void Execute(LocalPluginContext localcontext)
        {

            var service = localcontext.OrganizationService;
            var inputparameters = localcontext.PluginExecutionContext.InputParameters;
            var outputparameters = localcontext.PluginExecutionContext.OutputParameters;

            var xrmcontext = new XrmContext(service);

            var settingname = inputparameters["SettingName"] as string;

            var appname = inputparameters["AppName"] as string;

            //default values
            outputparameters["Exists"] = false;
            outputparameters["ValueString"] = string.Empty;
            

            var definition = xrmcontext.GetSettingDefinitionFor(settingname);
            if (definition != null)
            {
                outputparameters["Exists"] = true;

                var appmodule = xrmcontext.GetAppModuleByUniqueName(appname);

                var organizationsetting = xrmcontext.GetOrganizationSettingFor(definition);

                var appsetting = xrmcontext.GetAppSettingFor(definition, appmodule);

                var valuestring = appsetting != null ?
                                    appsetting.Value :
                                    (organizationsetting != null ?
                                        organizationsetting.Value :
                                        definition.DefaultValue);

                outputparameters["ValueString"] = valuestring;
                switch (definition.DataType)
                {
                    case settingdefinition_datatype.Boolean:

                        outputparameters["ValueBool"] = valuestring.ToLower() == "true";
                        break;

                    case settingdefinition_datatype.Number:
                        outputparameters["ValueDecimal"] = decimal.Parse(valuestring);

                        break;

                }

                outputparameters["IsOverridable"] = definition.IsOverridable ?? false;

                outputparameters["Type"] = new OptionSetValue((int)definition.DataType);
                outputparameters["TypeName"] = service.GetOptionsetText(SettingDefinition.EntityLogicalName, "datatype", (int)definition.DataType);

                outputparameters["ReleaseLevel"] = new OptionSetValue((int)definition.ReleaseLevel);
                outputparameters["ReleaseLevelName"] = service.GetOptionsetText(SettingDefinition.EntityLogicalName, "releaselevel", (int)definition.ReleaseLevel);

                outputparameters["OverridableLevel"] = new OptionSetValue((int)definition.OverridableLevel);
                outputparameters["OverridableLevelName"] = service.GetOptionsetText(SettingDefinition.EntityLogicalName, "overridablelevel", (int)definition.OverridableLevel);

                
            }
        }
        
    }
}
