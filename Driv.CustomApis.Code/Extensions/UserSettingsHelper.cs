using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace XrmVision.Extensions.Extensions
{
    public static class UserSettingsHelper
    {


        public static int UserLocaleId(this IOrganizationService service, Guid userid)
        {
           
            var fetch = new FetchExpression("<fetch>" +
                                            "<entity name = 'usersettings'>" +
                                            "<attribute name = 'systemuserid'/>" +
                                            "<attribute name = 'uilanguageid'/>" +
                                            "<filter>" +
                                            $"<condition attribute = 'systemuserid' operator= 'eq' value = '{userid}'/>" +
                                            "</filter>" +
                                            "</entity>" +
                                            "</fetch>");

            var results = service.RetrieveMultiple(fetch);
            if (results.Entities.Count > 0)
            {
                return (int)results[0].Attributes["uilanguageid"];
            }

            return 1033;  //DEFAULT ENGLISH
            
        }


        public static int UserTimeZone(this IOrganizationService service, Guid userid)
        {
            
                var fetch = new FetchExpression("<fetch>" +
                                                "<entity name = 'usersettings'>" +
                                                "<attribute name = 'systemuserid'/>" +
                                                "<attribute name = 'timezonecode'/>" +
                                                "<filter>" +
                                                $"<condition attribute = 'systemuserid' operator= 'eq' value = '{userid}'/>" +
                                                "</filter>" +
                                                "</entity>" +
                                                "</fetch>");

                var results = service.RetrieveMultiple(fetch);
                if (results.Entities.Count > 0)
                {
                    return (int)results[0].Attributes["timezonecode"];
                }

                return 0;
          
        }



        public static DateTime LocalTimeFromUtcTimeFor(this IOrganizationService service, DateTime utcTime, Guid userid)
        {
            var timezone = service.UserTimeZone(userid);
            return service.LocalTimeFromUtcTimeFor(utcTime, timezone);
        }

        public static DateTime LocalTimeFromUtcTimeFor(this IOrganizationService service, DateTime utcTime, int timezone)
        {
            var request = new LocalTimeFromUtcTimeRequest
            {
                TimeZoneCode = timezone,
                UtcTime = utcTime.ToUniversalTime()
            };

            var response = (LocalTimeFromUtcTimeResponse)service.Execute(request);

            
            return response.LocalTime;
        }

        

    }
}
