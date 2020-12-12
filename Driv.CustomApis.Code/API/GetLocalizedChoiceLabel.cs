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
    public class GetLocalizedChoiceLabel : PluginBase
    {
        public GetLocalizedChoiceLabel()
        {
            RegisterCustomApi("driv_GetLocalizedChoiceLabel", Execute);
        }

        public void Execute(LocalPluginContext localcontext)
        {

            var service = localcontext.OrganizationService;
            var inputparameters = localcontext.PluginExecutionContext.InputParameters;
            var outputparameters = localcontext.PluginExecutionContext.OutputParameters;


            var entityname = inputparameters["EntityName"] as string;
            var attributename = inputparameters["AttributeName"] as string;
            var choicevalue = inputparameters["ChoiceValue"] as int?;
            var langcode = inputparameters["LangCode"] as int?;

            //todo better error handling

            var objecttypecode = service.GetObjectTypeCode(entityname);
            var label = service.GetLocalizedOptionsetLabel(objecttypecode, attributename, choicevalue.Value, langcode.Value);

            outputparameters["Exists"] = !string.IsNullOrEmpty(label);
            outputparameters["Value"] = label;
            

            
        }

    }
}
