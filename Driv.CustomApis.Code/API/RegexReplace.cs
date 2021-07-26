using Driv.CustomApis.Helpers;
using Driv.CustomApis.Models;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XrmVision.Extensions.Extensions;

namespace Driv.CustomApis.API
{
    public class RegexReplace : PluginBase
    {
        public RegexReplace()
        {
            RegisterCustomApi("driv_RegexReplace", Execute);
        }

        public void Execute(LocalPluginContext localcontext)
        {


            var inputparameters = localcontext.PluginExecutionContext.InputParameters;
            var outputparameters = localcontext.PluginExecutionContext.OutputParameters;

            var inputstring = (string)inputparameters["InputString"];
            var regex = (string)inputparameters["Regex"];
            var replacestring = (string)inputparameters["ReplaceString"];

            var outputstring =  Regex.Replace(inputstring, regex, replacestring);

            outputparameters["OutputString"] = outputstring;


        }
    }
}
