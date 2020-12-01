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
    public static class ShareHelper
    {
        public static void ShareRecord(this OrganizationServiceContext context, EntityReference targetRef, EntityReference shareToRef)
        {
            var grantAccessRequest = new GrantAccessRequest
            {
                PrincipalAccess = new PrincipalAccess
                {
                    AccessMask = AccessRights.ReadAccess,
                    Principal = shareToRef
                },
                Target = targetRef
            };

            context.Execute(grantAccessRequest);
        }

        public static void UnShareRecord(this OrganizationServiceContext context, EntityReference targetRef, EntityReference unShareToRef)
        {
            var modifyAccessRequest = new ModifyAccessRequest
            {
                PrincipalAccess = new PrincipalAccess
                {
                    AccessMask = AccessRights.None,
                    Principal = unShareToRef
                },
                Target = targetRef
            };

            context.Execute(modifyAccessRequest);
        }
    }
}