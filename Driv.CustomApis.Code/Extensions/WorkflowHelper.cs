using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using XrmVision.Extensions.Extensions;

namespace XrmVision.Extensions.Extensions
{
    public static class WorkflowHelper
    {
        public static void ExecuteWorkflow(this IOrganizationService service, Guid entityId, Guid workflowid)
        {
            var request = GetWorkflowRequest(entityId, workflowid);
            service.Execute(request); //run the workflow
        }

        private static ExecuteWorkflowRequest GetWorkflowRequest(Guid entityId, Guid workflowid)
            => new ExecuteWorkflowRequest
            {
                EntityId = entityId,
                WorkflowId = workflowid
            };

        public static IEnumerable<ExecuteWorkflowRequest> GetWorkflowRequests(IEnumerable<Guid> entityids,
            Guid workflowid)
            => entityids.Select(entityid => GetWorkflowRequest(entityid, workflowid));

        public static IEnumerable<ExecuteWorkflowRequest> GetWorkflowRequests(EntityReference entityreference,
            Guid workflowid, int repeat)
        {
            var workflowrequests = new List<ExecuteWorkflowRequest>();
            for (int i = 0; i < repeat; i++)
            {
                workflowrequests.Add(GetWorkflowRequest(entityreference.Id, workflowid));
            }
            return workflowrequests.AsEnumerable();
        }


        public static void ExecuteWorkflows(this IOrganizationService service, IEnumerable<Guid> entityids,
            Guid workflowid, int batchsize = 1000)
        {

            var requests = GetWorkflowRequests(entityids, workflowid);
            service.ExecuteMultiple(requests);

        }

        public static void ExecuteWorkflows(this IOrganizationService service, EntityReference entityreference,
            Guid workflowid, int repeat, int batchsize = 1000)
        {

            var requests = GetWorkflowRequests(entityreference, workflowid, repeat);
            service.ExecuteMultiple(requests);

        }


    }

}