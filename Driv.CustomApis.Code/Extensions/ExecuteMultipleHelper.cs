using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace XrmVision.Extensions.Extensions
{
    public static class ExecuteMultipleHelper
    {
        public static void ExecuteMultiple(this IOrganizationService service, IEnumerable<OrganizationRequest> requests, int batchsize = 1000)
        {

     
            while (requests.Any())
            {
                var batch = requests.Take(batchsize);
                requests = requests.Skip(batchsize);

                var requestCollection = new OrganizationRequestCollection();
                requestCollection.AddRange(batch);

                var executemultiple = new ExecuteMultipleRequest
                {
                    Requests = requestCollection,
                    Settings = new ExecuteMultipleSettings()
                    {
                        ContinueOnError = false,
                        ReturnResponses = false
                    }
                };
                service.Execute(executemultiple);  //run the workflows

            }
      
        }
    }
}
