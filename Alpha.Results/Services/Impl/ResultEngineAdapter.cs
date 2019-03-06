namespace Alpha.Results.Site.Services.Impl
{
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using System.Collections.Generic;
    using System.IO;

    public class ResultEngineAdapter : IResultEngineAdapter
    {
        public MemoryStream ExcelParser<T>(IList<T> dataList, ResultSetting setting)
        {
            return ResultEngine.ExcelParser<T>(dataList, setting);
        }

        public string HtmlGameDetailParser(dynamic data, ResultSetting setting)
        {
            return ResultEngine.HtmlGameDetailParser(data, setting);
        }

        public string HtmlParser<T>(IList<T> dataList, ResultSetting setting)
        {
            return ResultEngine.HtmlParser<T>(dataList, setting);
        }
    }
}