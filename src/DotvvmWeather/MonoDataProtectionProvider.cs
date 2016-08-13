using System;
using System.Reflection;
using Microsoft.Owin.Security.DataProtection;

namespace DotvvmWeather
{
    public class MonoDataProtectionProvider : IDataProtectionProvider
    {
        private string appName;

        public MonoDataProtectionProvider() : this(Assembly.GetExecutingAssembly().GetName().Name)
        {
        }

        public MonoDataProtectionProvider(string appName)
        {
            if (appName == null)
            {
                throw new ArgumentNullException("appName");
            }
            this.appName = appName;
        }

        public IDataProtector Create(params string[] purposes)
        {
            return new MonoDataProtector(appName, purposes);
        }
    }
}