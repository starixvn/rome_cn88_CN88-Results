namespace Alpha.Results.Site.Services
{
    using Fanex.Result.Core.Base;
    using System.Collections.Generic;
    using System.IO;

    public interface IResultEngineAdapter
    {
        MemoryStream ExcelParser<T>(IList<T> dataList, ResultSetting setting);

        string HtmlGameDetailParser(dynamic data, ResultSetting setting);

        string HtmlParser<T>(IList<T> dataList, ResultSetting setting);
    }
}
