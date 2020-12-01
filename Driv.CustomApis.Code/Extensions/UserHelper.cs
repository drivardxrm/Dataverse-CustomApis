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
    public static class UserHelper
    {


        public static bool isUserDisabled(this IOrganizationService service, Guid userid)
        {
           
            var fetch = new FetchExpression("<fetch>" +
                                                "<entity name = 'systemuser'>" +
                                                    "<attribute name = 'systemuserid'/>" +
                                                    "<attribute name = 'isdisabled'/>" +
                                                    "<filter>" +
                                                        $"<condition attribute = 'systemuserid' operator= 'eq' value = '{userid}'/>" +
                                                    "</filter>" +
                                                "</entity>" +
                                            "</fetch>");

            var results = service.RetrieveMultiple(fetch);
            if (results.Entities.Count > 0)
            {
                return (bool)results[0].Attributes["isdisabled"];
            }

            return true; 
            
        }

    }
}
