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
    public class DateInfo : PluginBase
    {
        public DateInfo()
        {
            RegisterCustomApi("driv_DateInfo", Execute);
        }

        public void Execute(LocalPluginContext localcontext)
        {


            var inputparameters = localcontext.PluginExecutionContext.InputParameters;
            var outputparameters = localcontext.PluginExecutionContext.OutputParameters;

            var date = (DateTime)inputparameters["Date"];


            outputparameters["Year"] = date.Year;
            outputparameters["Month"] = date.Month;
            outputparameters["Day"] = date.Day;
            outputparameters["DayOfYear"] = date.DayOfYear;
            outputparameters["DayOfWeek"] = (int)date.DayOfWeek;
            outputparameters["IsLeapYear"] = DateTime.IsLeapYear(date.Year);
            outputparameters["IsLastDayOfMonth"] = date.IsLastDayOfMonth();


        }
    }
}
