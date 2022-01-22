using Microsoft.Xrm.Sdk;
using System.Linq;

namespace Driv.CustomApis.Helpers
{
    public static class EnvironmentVariableHelper
    {
        public static EnvironmentVariableDefinition GetEnvironmentVariableDefinition(this XrmContext xrmcontext, string key)
            => xrmcontext.EnvironmentVariableDefinitionSet.SingleOrDefault(e => e.SchemaName == key);

        public static EnvironmentVariableValue GetEnvironmentVariableValue(this XrmContext xrmcontext, EnvironmentVariableDefinition definition)
            => xrmcontext.EnvironmentVariableValueSet.SingleOrDefault(v => v.EnvironmentVariableDefinitionId != null &&
                                                                            v.EnvironmentVariableDefinitionId.Id == definition.Id);

        public static RetrieveEnvironmentVariableSecretValueResponse GetEnvironmentVariableSecretValue(this IOrganizationService service, string key)
        {
            var request = new RetrieveEnvironmentVariableSecretValueRequest
            {
                EnvironmentVariableName = key
            };
            return (RetrieveEnvironmentVariableSecretValueResponse)service.Execute(request);
        }

    }
}
