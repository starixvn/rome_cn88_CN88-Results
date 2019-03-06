namespace Alpha.Results.Site.Services.Impl.Products
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using Alpha.Results.Site.Criteria;
    using Alpha.Results.Site.DataAccesses;
    using DataAccesses.Impl;
    using Fanex.Common.Helper;
    using Fanex.Result.Core.Base;
    using Models.Requests;

    /// <summary>
    /// The class VirtualRacingResultService.
    /// </summary>
    public class VirtualRacingResultService : IProductResultService
    {
        private readonly IVirtualRacingResultDataAccess _racingResultDataAccess;
        private readonly IResultEngineAdapter _resultEngineAdapter;

        public VirtualRacingResultService(
            IVirtualRacingResultDataAccess racingResultDataAccess = null,
            IResultEngineAdapter resultEngineAdapter = null)
        {
            //// Still using poorman DI here because result service factory need to create it manual
            _racingResultDataAccess = racingResultDataAccess ?? new VirtualRacingResultDataAccess();
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

            var criteria = GetCriteria(request);
            var results = _racingResultDataAccess.GetVirtualRacingResults(criteria).ToList();

            return _resultEngineAdapter.HtmlParser(results, resultSetting);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Reviewed")]
        public MemoryStream ExportExcel(ExcelNormalResultQuery request)
        {
            Check.Argument.IsNotNull(request, "request");

            var results = _racingResultDataAccess.GetVirtualRacingResults(GetCriteria(request)).ToList();

            var resultSetting = new ResultSetting
            {
                SportType = (SportType)request.SportId,
                LanguageCode = request.Language,
                ExcelTitle = request.ExcelTitle,
                ExcelTemplatePath = request.TemplatePath,
                VisibleMatchCodeIDColumn = false,
                HighlightWinnerTeam = true
            };

            return _resultEngineAdapter.ExcelParser(results, resultSetting);
        }

        private static GetVirtualRacingResultCriteria GetCriteria(NormalResultQuery request)
        {
            return new GetVirtualRacingResultCriteria
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

            var resultSetting = new ResultSetting
            {
                SportType = (SportType)detailsRequest.SportId,
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

            var criteria = new GetRacingResultDetailsCriteria
            {
                MatchId = detailsRequest.MatchId,
                RaceNo = GetRaceNumber(detailsRequest.Race)
            };
            var results = _racingResultDataAccess.GetVirtualRacingResultDetails(criteria);

            return _resultEngineAdapter.HtmlParser(results, resultSetting);
        }

        private int GetRaceNumber(string raceNumber)
        {
            if (string.IsNullOrEmpty(raceNumber) || raceNumber == "0")
            {
                return 0;
            }

            try
            {
                return int.Parse(raceNumber);
            }
            catch (Exception exception)
            {
                if (exception is NotFiniteNumberException || exception is FormatException)
                {
                    return 0;
                }

                throw;
            }
        }
    }
}
