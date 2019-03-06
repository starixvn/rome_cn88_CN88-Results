namespace Alpha.Results.Site.Services.Impl.Products
{
    using System;
    using System.IO;
    using Alpha.Results.Site.Criteria;
    using DataAccesses;
    using DataAccesses.Impl;
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Models.Requests;

    public class MetricGamingResultService : IProductResultService
    {
        private readonly IMetricGamingResultDataAccess _dataAccess;
        private readonly IResultEngineAdapter _resultEngineAdapter;

        public MetricGamingResultService(
            IMetricGamingResultDataAccess dataAccess = null,
            IResultEngineAdapter resultEngineAdapter = null)
        {
            //// Still using poorman DI here because result service factory need to create it manual
            _dataAccess = dataAccess ?? new MetricGamingResultDataAccess();
            _resultEngineAdapter = resultEngineAdapter ?? new ResultEngineAdapter();
        }

        public string GetResult(NormalResultQuery request)
        {
            throw new NotImplementedException();
        }

        public MemoryStream ExportExcel(ExcelNormalResultQuery request)
        {
            throw new NotImplementedException();
        }

        public string GetResultDetails(NormalResultDetailsQuery detailsRequest)
        {
            if (detailsRequest == null)
            {
                throw new ArgumentNullException(nameof(detailsRequest), "Request cannot be null");
            }

            var criteria = new GetMetricGamingResultCriteria
            {
                TransId = detailsRequest.RefNum,
                BetType = detailsRequest.BetTypeId,
                BetId = detailsRequest.BetId,
                SportType = detailsRequest.SportId
            };

            var results = _dataAccess.GetResults(criteria);

            var resultSetting = new ResultSetting
            {
                SportType = SportType.Soccer,
                IsOutright = false,
                IsParlayPopUp = false,
                IsPopUp = true,
                LanguageCode = "en-US",
                VisibleMatchCodeIDColumn = false,
                VisiblePopUpTitle = true,
                HighlightWinnerTeam = true
            };

            return _resultEngineAdapter.HtmlParser(results, resultSetting);
        }
    }
}