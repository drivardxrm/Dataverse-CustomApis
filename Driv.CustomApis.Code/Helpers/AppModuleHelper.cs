using System.Linq;

namespace Driv.CustomApis.Helpers
{
    public static class AppModuleHelper
    {
        public static AppModule GetAppModuleByUniqueName(this XrmContext xrmcontext, string appname)
            => string.IsNullOrEmpty(appname) ? 
                        null : 
                        xrmcontext.AppModuleSet.SingleOrDefault(a => a.UniqueName == appname);
    }
}
