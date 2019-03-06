namespace Alpha.Results.Site.Services.Impl.Products
{
    using System.IO;
    using System.Linq;
    using Alpha.Results.Site.Criteria;
    using Alpha.Results.Site.DataAccesses;
    using Constants;
    using DataAccesses.Impl;
    using Fanex.Common.Helper;
    using Fanex.Result.Core.Base;
    using Models.Requests;

    public class Lucky3ResultService : IProductResultService
    {
        private readonly ILucky3ResultDataAccess lucky3ResultDataAccess;
        private readonly IResultEngineAdapter resultEngineAdapter;

        public Lucky3ResultService(
            ILucky3ResultDataAccess lucky3ResultDataAccess = null,
            IResultEngineAdapter resultEngineAdapter = null)
        {
            this.lucky3ResultDataAccess = lucky3ResultDataAccess ?? new Lucky3ResultDataAccess();
            this.resultEngineAdapter = resultEngineAdapter ?? new ResultEngineAdapter();
        }

        public string GetResult(NormalResultQuery request)
        {
            Check.Argument.IsNotNull(request, "request");

            var resultSetting = new ResultSetting
            {
                SportType = (SportType)request.SportId,
                LanguageCode = request.Language,
                VisibleDetailedLink = true,
                VisibleFreeHeader = true,
                VisibleMatchCodeIDColumn = false,
                HighlightWinnerTeam = true
            };

            var results = lucky3ResultDataAccess.GetResults(GetCriteria(request)).ToList();

            return resultEngineAdapter.HtmlParser(results, resultSetting);
        }

        public MemoryStream ExportExcel(ExcelNormalResultQuery request)
        {
            Check.Argument.IsNotNull(request, "request");

            var resultSetting = new ResultSetting
            {
                SportType = (SportType)request.SportId,
                LanguageCode = request.Language,
                ExcelTitle = request.ExcelTitle,
                ExcelTemplatePath = request.TemplatePath,
                VisibleMatchCodeIDColumn = false,
                HighlightWinnerTeam = true
            };

            var results = lucky3ResultDataAccess.GetResults(GetCriteria(request)).ToList();

            return resultEngineAdapter.ExcelParser(results, resultSetting);
        }

        public string GetResultDetails(NormalResultDetailsQuery detailsRequest)
        {
            Check.Argument.IsNotNull(detailsRequest, "detailsRequest");

            var criteria = new GetNumberGameResultDetailsCriteria
            {
                MatchId = detailsRequest.MatchId
            };

            var resultSetting = new ResultSetting
            {
                SportType = SportType.Lucky3,
                IsOutright = false,
                IsParlayPopUp = false,
                IsPopUp = true,
                VisiblePopUpTitle = false,
                LanguageCode = detailsRequest.Language,
                VisibleMatchCodeIDColumn = false,
                VisibleFreeHeader = true,
                VisibleDetailedLink = true,
                HighlightWinnerTeam = true
            };

            var results = lucky3ResultDataAccess.GetResultDetails(criteria);

            return resultEngineAdapter.HtmlParser(results, resultSetting);
        }

        private static GetNumberGameResultCriteria GetCriteria(NormalResultQuery request)
        {
            return new GetNumberGameResultCriteria
            {
                FromDate = request.ResultDate,
                LeagueId = request.LeagueId,
                Language = request.Language,
                SportType = SportTypeIdConstants.Lucky3
            };
        }
    }
}
