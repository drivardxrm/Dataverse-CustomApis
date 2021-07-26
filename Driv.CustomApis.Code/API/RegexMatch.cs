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
    public class RegexMatch : PluginBase
    {
        public RegexMatch()
        {
            RegisterCustomApi("driv_RegexMatch", Execute);
        }

        public void Execute(LocalPluginContext localcontext)
        {


            var inputparameters = localcontext.PluginExecutionContext.InputParameters;
            var outputparameters = localcontext.PluginExecutionContext.OutputParameters;

            var inputstring = (string)inputparameters["InputString"];
            var regex = (string)inputparameters["Regex"];

            var match =  Regex.Match(inputstring, regex);
            var matchfound = match.Success;

            var matches = new List<RegexMatchModel>();
            
            while (match.Success)
            {
                matches.Add(new RegexMatchModel { 
                    Index = match.Index,
                    Value = match.Value
                });
                match = match.NextMatch();
            }

            outputparameters["MatchFound"] = matchfound;
            outputparameters["Matches"] = JsonHelper.SerializeJSon(matches);


        }
    }
}
