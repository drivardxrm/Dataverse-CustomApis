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
    public class RemoveDiacritics : PluginBase
    {
        public RemoveDiacritics()
        {
            RegisterCustomApi("driv_RemoveDiacritics", Execute);
        }

        public void Execute(LocalPluginContext localcontext)
        {


            var inputparameters = localcontext.PluginExecutionContext.InputParameters;
            var outputparameters = localcontext.PluginExecutionContext.OutputParameters;

            var input = (string)inputparameters["InputString"];
            

   
            outputparameters["OutputString"] = input.RemoveDiacritics();

        }
    }
}
