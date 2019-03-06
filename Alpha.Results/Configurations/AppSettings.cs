namespace Alpha.Results.Site.Configurations
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;

    public static class AppSettings
    {
        [ThreadStatic]
        private static NameValueCollection appSettingValues;

        internal static NameValueCollection AppSettingValues
        {
            get { return appSettingValues ?? ConfigurationManager.AppSettings; }

            set { appSettingValues = value; }
        }

        public static string NG2SiteAppPath => GetConfigValue("NG2SiteVirtualPath");

        public static bool LocalTest => GetConfigValue("LocalTest") == "1";

        public static bool Disable151_31 => GetConfigValue("Disable151_31") == "1";

        public static string RemoteRefURL => GetConfigValue("RemoteRefURL");

        public static string GetConfigValue(string configName) => AppSettingValues[configName];
    }
}
