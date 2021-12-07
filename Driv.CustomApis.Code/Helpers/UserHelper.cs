using System.Linq;

namespace Driv.CustomApis.Helpers
{
    public static class UserHelper
    {
        public static TimeZoneDefinition GetTimezoneDefinitionFor(this XrmContext xrmContext, SystemUser user)
        {
            var usersettings = xrmContext.GetUserSettingsFor(user);
            return xrmContext.TimeZoneDefinitionSet.SingleOrDefault(t => t.TimeZoneCode == usersettings.TimeZoneCode);
        }

        public static UserSettings GetUserSettingsFor(this XrmContext xrmContext, SystemUser user)
            => xrmContext.UserSettingsSet.SingleOrDefault(s => s.SystemUserId == user.Id);
    }
}
