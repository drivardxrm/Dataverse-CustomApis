using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
