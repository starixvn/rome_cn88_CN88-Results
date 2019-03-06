namespace Alpha.Results.Site.Services.Impl.Products
{
    using System;
    using System.IO;
    using System.Linq;
    using Alpha.Results.Site.Criteria;
    using Fanex.Common.Helper;
    using Fanex.Data.Repository;
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.GoldDeluxe.DTO;
    using Models.Requests;

    public class GoldDeluxeResultService : IProductResultService
    {
        private readonly IDynamicRepository _dynamicRepository;
        private readonly IResultEngineAdapter _resultEngineAdapter;

        public GoldDeluxeResultService(
            IDynamicRepository dynamicRepository = null,
            IResultEngineAdapter resultEngineAdapter = null)
        {
            //// Still using poorman DI here because result service factory need to create it manual
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
            _resultEngineAdapter = resultEngineAdapter ?? new ResultEngineAdapter();
        }

        public MemoryStream ExportExcel(ExcelNormalResultQuery request)
        {
            throw new NotImplementedException();
        }

        public string GetResult(NormalResultQuery request)
        {
            throw new NotImplementedException();
        }

        public string GetResultDetails(NormalResultDetailsQuery detailsRequest)
        {
            Check.Argument.IsNotNull(detailsRequest, "detailsRequest");

            var criteria = new GetGoldDeluxeResultCriteria
            {
                TransId = detailsRequest.RefNum,
                BetType = detailsRequest.BetTypeId
            };

            var goldDeluxeResults = _dynamicRepository
                .Fetch<GoldDeluxe>(criteria)
                .Select(result =>
                {
                    result.Bettype = detailsRequest.BetTypeId;
                    return result;
                }).ToList();

            var resultSetting = new ResultSetting
            {
                SportType = SportType.GoldDeluxe,
                IsOutright = false,
                IsParlayPopUp = false,
                IsPopUp = true,
                LanguageCode = detailsRequest.Language,
                VisibleMatchCodeIDColumn = false,
                HighlightWinnerTeam = true
            };

            return _resultEngineAdapter.HtmlParser(goldDeluxeResults, resultSetting);
        }
    }
}