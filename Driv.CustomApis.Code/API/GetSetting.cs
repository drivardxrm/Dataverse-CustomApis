using Driv.CustomApis.Helpers;
using Microsoft.Crm.Sdk.Messages;
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

                //Error handling
                if (!string.IsNullOrEmpty(appname))
                {
                    var appmodule = xrmcontext.GetAppModuleByUniqueName(appname);
                    if (appmodule == null)
                    {
                        throw new InvalidPluginExecutionException($"App with name {appname} doesn't exists");
                    }
                }

                // use OOB RetrieveSEtting function
                var settingdetail = service.RetrieveSetting(settingname, appname);

                outputparameters["ValueString"] = settingdetail.Value;

                switch (definition.DataType)
                {
                    case settingdefinition_datatype.Boolean:

                        outputparameters["ValueBool"] = settingdetail.Value.ToLower() == "true";
                        break;

                    case settingdefinition_datatype.Number:
                        outputparameters["ValueDecimal"] = decimal.Parse(settingdetail.Value);

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
