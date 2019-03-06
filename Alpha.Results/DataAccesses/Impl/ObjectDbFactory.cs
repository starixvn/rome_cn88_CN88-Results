namespace Alpha.Results.Site.DataAccesses.Impl
{
    using System;
    using Fanex.Data;

    public static class ObjectDbFactory
    {
        [ThreadStatic]
        private static Func<string, IObjectDb> createInstance;

        [ThreadStatic]
        private static Func<string, int, IObjectDb> createTimeOutInstance;

        public static Func<string, IObjectDb> CreateInstance
        {
            get { return createInstance ?? ((settingKey) => new ObjectDb(settingKey)); }
            internal set { createInstance = value; }
        }

        public static Func<string, int, IObjectDb> CreateTimeOutInstance
        {
            get { return createTimeOutInstance ?? ((settingKey, commandTimeOut) => new ObjectDb(settingKey, commandTimeOut)); }
            internal set { createTimeOutInstance = value; }
        }
    }
}
