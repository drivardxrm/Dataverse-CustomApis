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

            

            


            var isLastDayofMonth = false;
            switch (date.Month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    if (date.Day == 31)
                    {
                        isLastDayofMonth = true;
                    }
                    break;
                case 2:
                    if ((DateTime.IsLeapYear(date.Year) && date.Day == 29)
                        ||
                        (!DateTime.IsLeapYear(date.Year) && date.Day == 28))
                    {
                        isLastDayofMonth = true;
                    }
                    
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    if (date.Day == 30)
                    {
                        isLastDayofMonth = true;
                    }
                    break;

            }


            outputparameters["Year"] = date.Year;
            outputparameters["Month"] = date.Month;
            outputparameters["Day"] = date.Day;
            outputparameters["DayOfYear"] = date.DayOfYear;
            outputparameters["DayOfWeek"] = (int)date.DayOfWeek;
            outputparameters["IsLeapYear"] = DateTime.IsLeapYear(date.Year);
            outputparameters["IsLastDayOfMonth"] = isLastDayofMonth;


        }
    }
}
