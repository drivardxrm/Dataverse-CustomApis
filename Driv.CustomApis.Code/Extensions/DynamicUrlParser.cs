using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XrmVision.Extensions.Extensions
{
    /// <summary>
    /// Used to parse the Dynamics CRM 'Record Url (Dynamic)' that can be created by workflows and dialogs
    /// </summary>
    public class DynamicUrlParser
    {
        public string Url { get; set; }
        public int EntityTypeCode { get; set; }
        public Guid Id { get; set; }

        /// <summary>
        /// Parse the dynamic url in constructor
        /// </summary>
        /// <param name="url"></param>
        public DynamicUrlParser(string url)
        {
            try
            {
                Url = url;
                var uri = new Uri(url);
                var found = 0;

                var parameters = uri.Query.TrimStart('?').Split('&');
                foreach (var param in parameters)
                {
                    var nameValue = param.Split('=');
                    switch (nameValue[0])
                    {
                        case "etc":
                            EntityTypeCode = int.Parse(nameValue[1]);
                            found++;
                            break;
                        case "id":
                            Id = new Guid(nameValue[1]);
                            found++;
                            break;
                    }
                    if (found > 1) break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Url '{url}' is incorrectly formated for a Dynamics CRM Dynamics Url", ex);
            }
        }

        /// <summary>
        /// Return the entity reference of the Record Url
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public EntityReference GetEntityReference(IOrganizationService service)
        {
            return new EntityReference(service.GetEntityLogicalName(EntityTypeCode), Id);
        }


    }

}