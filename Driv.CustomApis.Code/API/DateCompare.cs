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
    public class DateCompare : PluginBase
    {
        public DateCompare()
        {
            RegisterCustomApi("driv_DateCompare", Execute);
        }

        public void Execute(LocalPluginContext localcontext)
        {


            var inputparameters = localcontext.PluginExecutionContext.InputParameters;
            var outputparameters = localcontext.PluginExecutionContext.OutputParameters;

            var date1 = (DateTime)inputparameters["Date1"];
            var date2 = (DateTime)inputparameters["Date2"];

            var sameYear = date1.Year == date2.Year;
            var sameMonth = date1.Month == date2.Month;
            var sameDay = date1.Day == date2.Day;

            var timespan = date2.Date - date1.Date;

            outputparameters["SameYear"] = sameYear;
            outputparameters["SameMonth"] = sameMonth;
            outputparameters["SameDay"] = sameDay;
            outputparameters["DaysBetween"] = Math.Abs(timespan.Days);

        }
    }
}
