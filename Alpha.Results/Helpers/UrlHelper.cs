namespace Alpha.Results.Site.Helpers
{
    using System;
    using System.Web;
    using Configurations;
    using Web.Helpers;

    public static class UrlHelper
    {
        [ThreadStatic]
        private static string ng2ResultPath;

        [ThreadStatic]
        private static string currentAppPath;

        [ThreadStatic]
        private static string resultTemplatePath;

        public static string NG2ResultPath
        {
            get
            {
                return ng2ResultPath ?? AppPathHelper.GetAppPath(AppSettings.NG2SiteAppPath, "~/Result");
            }

            internal set
            {
                ng2ResultPath = value;
            }
        }

        public static string CurrentAppPath
        {
            get
            {
                return currentAppPath ?? HttpContext.Current.Response.ApplyAppPathModifier("~/");
            }

            internal set
            {
                currentAppPath = value;
            }
        }

        public static string ResultTemplatePath
        {
            get
            {
                return resultTemplatePath ?? HttpContext.Current.Server.MapPath(@"~/App_Data/Fanex.Result/");
            }

            internal set
            {
                resultTemplatePath = value;
            }
        }
    }
}
