namespace Alpha.Results.Site.Services.Impl.Products
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using Alpha.Results.Site.Criteria;
    using Alpha.Results.Site.DataAccesses;
    using DataAccesses.Impl;
    using Fanex.Common.Helper;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Models.Requests;

    /// <summary>
    /// The class SportsBookResultService.
    /// </summary>
    public class SportsbookResultService : IProductResultService
    {
        private readonly ISportsbookResultDataAccess _dataAccess;
        private readonly IResultEngineAdapter _resultEngineAdapter;

        public SportsbookResultService(
            ISportsbookResultDataAccess dataAccess = null,
            IResultEngineAdapter resultEngineAdapter = null)
        {
            //// Still using poorman DI here because result service factory need to create it manual
            _dataAccess = dataAccess ?? new SportsbookResultDataAccess();
            _resultEngineAdapter = resultEngineAdapter ?? new ResultEngineAdapter();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Reviewed")]
        public string GetResult(NormalResultQuery request)
        {
            Check.Argument.IsNotNull(request, "request");

            var criteria = GetCriteria(request);

            var results = request.SportId == 0
                ? new List<Sportsbook>()
                : _dataAccess.GetSportbookResults(criteria).ToList();

            var sportType = request.SportId > 0
                ? (SportType)request.SportId
                : SportType.Others;

            var resultSetting = new ResultSetting
            {
                SportType = sportType,
                LanguageCode = request.Language,
                VisibleDetailedLink = HasDetailResult(sportType),
                VisibleFreeHeader = true,
                VisibleMatchCodeIDColumn = false,
                HighlightWinnerTeam = true,
                EnableCheckScoreCalculated = true
            };

            return _resultEngineAdapter.HtmlParser(results, resultSetting).Replace("original-title", " title ");
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Reviewed")]
        public MemoryStream ExportExcel(ExcelNormalResultQuery request)
        {
            Check.Argument.IsNotNull(request, "request");

            var exportExcelSetting = new ResultSetting
            {
                SportType = (SportType)request.SportId,
                LanguageCode = request.Language,
                ExcelTitle = request.ExcelTitle,
                ExcelTemplatePath = request.TemplatePath,
                VisibleMatchCodeIDColumn = false,
                HighlightWinnerTeam = true,
                EnableCheckScoreCalculated = true
            };

            var results = _dataAccess.GetSportbookResults(GetCriteria(request)).ToList();

            return _resultEngineAdapter.ExcelParser(results, exportExcelSetting);
        }

        private static GetSportsbookResultCriteria GetCriteria(NormalResultQuery request)
        {
            return new GetSportsbookResultCriteria(request.MoveBetsDate)
            {
                FromDate = request.ResultDate,
                SportType = request.SportId,
                LeagueId = request.LeagueId
            };
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Reviewed")]
        public string GetResultDetails(NormalResultDetailsQuery detailsRequest)
        {
            Check.Argument.IsNotNull(detailsRequest, "detailsRequest");

            var criteria = new GetSportsbookResultDetailsCriteria
            {
                MatchId = detailsRequest.MatchId,
                BetType = detailsRequest.BetTypeId,
                SportType = detailsRequest.SportId,
                BetId = detailsRequest.BetId,
                IsViewResultFromBetList = detailsRequest.IsViewResultFromBetList,
                Handicap2 = detailsRequest.Hdp2
            };

            var results = _dataAccess.GetScoreInformation(criteria);

            var resultSetting = new ResultSetting
            {
                SportType = (SportType)detailsRequest.SportId,
                IsOutright = false,
                IsParlayPopUp = false,
                IsPopUp = true,
                IsPopupOpenFromBetList = detailsRequest.IsViewResultFromBetList,
                LanguageCode = detailsRequest.Language,
                VisibleMatchCodeIDColumn = false,
                HighlightWinnerTeam = true,
                EnableCheckScoreCalculated = true
            };

            return detailsRequest.IsViewResultFromBetList
                ? _resultEngineAdapter.HtmlParser(results, resultSetting).Replace("original-title", " title ")
                : _resultEngineAdapter.HtmlGameDetailParser(results[0], resultSetting).Replace("original-title", " title ");
        }


        private static bool HasDetailResult(SportType sportType)
        {
            var sportHasDetail = new List<SportType>
            {
                SportType.Soccer,
                SportType.Tennis,
                SportType.BasketBall,
                SportType.VirtualBasketBall,
                SportType.ESport
            };

            return sportHasDetail.Contains(sportType);
        }
    }
}
