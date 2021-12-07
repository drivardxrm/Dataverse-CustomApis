using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Driv.CustomApis.Helpers
{
    public static class ChoiceHelper
    {


        public static string GetLocalizedOptionsetLabel(this IOrganizationService service, int objecttypecode, string attributename, int choicevalue, int langcode)
        {
            
            var fetchXml = $@"
                            <fetch>
                              <entity name='stringmap'>
                                <filter>
                                  <condition attribute='objecttypecode' operator='eq' value='{objecttypecode}'/>
                                  <condition attribute='attributename' operator='eq' value='{attributename}'/>
                                  <condition attribute='attributevalue' operator='eq' value='{choicevalue}'/>
                                  <condition attribute='langid' operator='eq' value='{langcode}'/>
                                </filter>
                              </entity>
                            </fetch>";


            var query = new FetchExpression(fetchXml);

            var results = service.RetrieveMultiple(query);
            return results.Entities.Count > 0 ? (string)results[0].Attributes["value"] : string.Empty;
        }
    }
}
