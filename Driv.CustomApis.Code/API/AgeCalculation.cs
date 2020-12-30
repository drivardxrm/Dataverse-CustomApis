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
    public class AgeCalculation : PluginBase
    {
        public AgeCalculation()
        {
            RegisterCustomApi("driv_AgeCalculation", Execute);
        }

        public void Execute(LocalPluginContext localcontext)
        {


            var inputparameters = localcontext.PluginExecutionContext.InputParameters;
            var outputparameters = localcontext.PluginExecutionContext.OutputParameters;

            var birthday = (DateTime)inputparameters["Birthday"];
            var ageatdate = (DateTime)inputparameters["AgeAtDate"];

            var age = ageatdate.Year - birthday.Year;
            if (birthday > ageatdate.AddYears(-age))
            {
                age--;
            }
            outputparameters["Age"] = age;

        }
    }
}
