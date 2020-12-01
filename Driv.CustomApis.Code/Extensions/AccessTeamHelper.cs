using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XrmVision.Extensions.Extensions
{
    public static class AccessTeamHelper
    {
        public static AddUserToRecordTeamResponse AddUserToRecordTeam(this IOrganizationService service, EntityReference record, Guid userid, Guid templateid)
        {
            //USER CANNOT BE ADDDED IF DISABLED
            if (service.isUserDisabled(userid))
            {
                return null;
            }


            var addUserToRecordTeamRequest = new AddUserToRecordTeamRequest
            {
                Record = record,
                SystemUserId = userid,
                TeamTemplateId = templateid
            };

            
            return (AddUserToRecordTeamResponse)service.Execute(addUserToRecordTeamRequest);
        }

        public static RemoveUserFromRecordTeamResponse RemoveUserFromRecordTeam(this IOrganizationService service, EntityReference record, Guid userid, Guid templateid)
        {

            var removeUserFromRecordTeamRequest = new RemoveUserFromRecordTeamRequest
            {
                Record = record,
                SystemUserId = userid,
                TeamTemplateId = templateid
            };


            return (RemoveUserFromRecordTeamResponse)service.Execute(removeUserFromRecordTeamRequest);
        }
    }
}
