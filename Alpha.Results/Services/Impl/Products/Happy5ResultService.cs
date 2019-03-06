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
    using Fanex.Result.Core.Happy5.DTO;
    using Models.Requests;


    public class Happy5ResultService : IProductResultService
    {
        private readonly INumberGameResultDataAccess _numberGameResultDataAccess;
        private readonly IResultEngineAdapter _resultEngineAdapter;

        public Happy5ResultService(
            INumberGameResultDataAccess numberGameResultDataAccess = null,
            IResultEngineAdapter resultEngineAdapter = null)
        {
            _numberGameResultDataAccess = numberGameResultDataAccess ?? new NumberGameResultDataAccess();
            _resultEngineAdapter = resultEngineAdapter ?? new ResultEngineAdapter();
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

            var results = _numberGameResultDataAccess.GetNumberGameResults(GetCriteria(request));

            var happy5Results = results.Select(i =>
            new Happy5
            {
                Ball = i.Ball,
                EventDate = i.EventDate,
                EventStatus = i.EventStatus,
                IsPending = i.IsPending,
                KickOffTime = i.KickOffTime,
                LeagueId = i.LeagueId,
                MatchCode = i.MatchCode,
                MatchId = i.MatchId,
                Seq = i.Seg
            }).ToList();

            return _resultEngineAdapter.HtmlParser(happy5Results, resultSetting);
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

            var results = _numberGameResultDataAccess.GetNumberGameResults(GetCriteria(request));

            var happy5Results = results.Select(
                i => new Happy5
                {
                    Ball = i.Ball,
                    EventDate = i.EventDate,
                    EventStatus = i.EventStatus,
                    IsPending = i.IsPending,
                    KickOffTime = i.KickOffTime,
                    LeagueId = i.LeagueId,
                    MatchCode = i.MatchCode,
                    MatchId = i.MatchId,
                    Seq = i.Seg
                }).ToList();

            return _resultEngineAdapter.ExcelParser(happy5Results, resultSetting);
        }

        private static GetNumberGameResultCriteria GetCriteria(NormalResultQuery request)
        {
            return new GetNumberGameResultCriteria
            {
                FromDate = request.ResultDate,
                LeagueId = request.LeagueId,
                Language = request.Language,
                SportType = SportTypeIdConstants.Happy5
            };
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
                SportType = SportType.Happy5,
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

            var happy5Results = results.Select(
                i => new Happy5
                {
                    Ball = i.Ball,
                    EventDate = i.EventDate,
                    EventStatus = i.EventStatus,
                    IsPending = i.IsPending,
                    KickOffTime = i.KickOffTime,
                    LeagueId = i.LeagueId,
                    MatchCode = i.MatchCode,
                    MatchId = i.MatchId,
                    Seq = i.Seg
                }).ToList();

            return _resultEngineAdapter.HtmlParser(happy5Results, resultSetting);
        }
    }
}
