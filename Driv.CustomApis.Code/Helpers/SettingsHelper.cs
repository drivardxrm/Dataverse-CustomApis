using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driv.CustomApis.Helpers
{
    public static class SettingsHelper
    {
        public static SettingDefinition GetSettingDefinitionFor(this XrmContext xrmcontext, string settingname)
            => xrmcontext.SettingDefinitionSet.SingleOrDefault(s => s.UniqueName == settingname);

        public static OrganizationSetting GetOrganizationSettingFor(this XrmContext xrmcontext, SettingDefinition definition)
            => definition.IsOverridable == true &&
                definition.OverridableLevel == settingdefinition_overridablelevel.AppOnly ?
                null :
                xrmcontext.OrganizationSettingSet.SingleOrDefault(o => o.SettingDefinitionId != null &&
                                                                      o.SettingDefinitionId.Id == definition.Id);

        public static AppSetting GetAppSettingFor(this XrmContext xrmcontext, SettingDefinition definition, AppModule appmodule)
            => definition.IsOverridable == true &&
               definition.OverridableLevel != settingdefinition_overridablelevel.OrganizationOnly && 
               appmodule == null ? null : 
                            xrmcontext.AppSettingSet.SingleOrDefault(a => a.SettingDefinitionId != null &&
                                                                            a.SettingDefinitionId.Id == definition.Id &&
                                                                            a.ParentAppModuleId != null &&
                                                                            a.ParentAppModuleId.Id == appmodule.Id);
    }
}
