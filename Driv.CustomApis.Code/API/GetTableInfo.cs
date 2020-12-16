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
    public class GetTableInfo : PluginBase
    {
        public GetTableInfo()
        {
            RegisterCustomApi("driv_GetTableInfo", Execute);
        }

        public void Execute(LocalPluginContext localcontext)
        {

            var service = localcontext.OrganizationService;
            var inputparameters = localcontext.PluginExecutionContext.InputParameters;
            var outputparameters = localcontext.PluginExecutionContext.OutputParameters;

            var logicalname = inputparameters["LogicalName"] as string;


            //todo proper error handling  
            try
            {
                var metadata = service.GetMetadata(logicalname);

                outputparameters["Exists"] = metadata != null;
                outputparameters["DisplayName"] = metadata?.DisplayName.UserLocalizedLabel.Label;
                outputparameters["SchemaName"] = metadata?.SchemaName;
                outputparameters["CollectionName"] = metadata?.LogicalCollectionName;
                outputparameters["CollectionSchemaName"] = metadata?.CollectionSchemaName;
                outputparameters["PrimaryIdAttribute"] = metadata?.PrimaryIdAttribute;
                outputparameters["PrimaryNameAttribute"] = metadata?.PrimaryNameAttribute;
                outputparameters["ObjectTypeCode"] = metadata?.ObjectTypeCode;
            }
            catch (Exception)
            {

                outputparameters["Exists"] = false;
            }
            



        }

    }
}
