namespace Alpha.Results.Site.Services.Impl.Products
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using Alpha.Results.Site.Criteria;
    using Alpha.Results.Site.DataAccesses;
    using Constants;
    using DataAccesses.Impl;
    using Fanex.Common.Helper;
    using Fanex.Result.Core.Base;
    using Models.Requests;

    /// <summary>
    /// The class NumberGameResultService.
    /// </summary>
    public class NumberGameResultService : IProductResultService
    {
        private readonly INumberGameResultDataAccess _numberGameResultDataAccess;
        private readonly IResultEngineAdapter _resultEngineAdapter;

        public NumberGameResultService(
            INumberGameResultDataAccess numberGameResultDataAccess = null,
            IResultEngineAdapter resultEngineAdapter = null)
        {
            //// Still using poorman DI here because result service factory need to create it manual
            _numberGameResultDataAccess = numberGameResultDataAccess ?? new NumberGameResultDataAccess();
            _resultEngineAdapter = resultEngineAdapter ?? new ResultEngineAdapter();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Reviewed")]
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

            var results = _numberGameResultDataAccess.GetNumberGameResults(GetCriteria(request)).ToList();

            return _resultEngineAdapter.HtmlParser(results, resultSetting);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Reviewed")]
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

            var results = _numberGameResultDataAccess.GetNumberGameResults(GetCriteria(request)).ToList();

            return _resultEngineAdapter.ExcelParser(results, resultSetting);
        }

        private static GetNumberGameResultCriteria GetCriteria(NormalResultQuery request)
        {
            return new GetNumberGameResultCriteria
            {
                FromDate = request.ResultDate,
                LeagueId = request.LeagueId,
                Language = request.Language,
                SportType = SportTypeIdConstants.NumberGame
            };
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Reviewed")]
        public string GetResultDetails(NormalResultDetailsQuery detailsRequest)
        {
            Check.Argument.IsNotNull(detailsRequest, "detailsRequest");

            var criteria = new GetNumberGameResultDetailsCriteria
            {
                MatchId = detailsRequest.MatchId
            };

            var resultSetting = new ResultSetting
            {
                SportType = SportType.NumberGame,
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

            var results = _numberGameResultDataAccess.GetNumberGameResultDetails(criteria);

            return _resultEngineAdapter.HtmlParser(results, resultSetting);
        }
    }
}
