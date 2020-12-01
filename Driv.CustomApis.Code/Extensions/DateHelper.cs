using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XrmVision.Extensions.Extensions
{
    public static class DateHelper
    {
        public static int DaysBetween(DateTime date1, DateTime date2)
        {
            var timespan = date2.Date - date1.Date;
            return Math.Abs(timespan.Days);
        }

        public static bool IsLastDayOfMonth(this DateTime date)
            => date.Day == DateTime.DaysInMonth(date.Year, date.Month);

    }
}
