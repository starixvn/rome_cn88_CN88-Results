namespace Alpha.Results.Site.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Models.Requests;

    public interface IOutrightResultService
    {
        IEnumerable<Outright> GetOutrights(DateTime fromDate, DateTime toDate, DateTime balanceUpDate);

        string GetResults(IEnumerable<Outright> outrights, OutrightResultQuery request);

        MemoryStream GetExcelResults(ExcelOutrightResultQuery request, DateTime balanceUpDate);
    }
}