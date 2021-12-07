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
    }
}
