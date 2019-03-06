namespace Alpha.Results.Site.Services
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Models.Requests;

    public interface INormalResultService
    {
        IEnumerable<Sport> GetSports(DateTime resultDate, IDictionary<int, bool> sportsStatus, DateTime moveBetsDate);

        IEnumerable<League> GetLeagues(DateTime resultDate, int sportId, DateTime moveBetsDate);

        string GetResults(NormalResultQuery request);

        string GetResultDetails(NormalResultDetailsQuery detailRequest);

        byte[] GetExcelResults(ExcelNormalResultQuery request);
    }
}