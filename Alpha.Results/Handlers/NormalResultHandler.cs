namespace Alpha.Results.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using App_GlobalResources;
    using Fanex.Common.Helper;
    using MediatR;
    using Models.Requests;
    using Models.Views;
    using Services;
    using Sunplus.Agent.Utilities.Extension;

    public class NormalResultHandler :
        IRequestHandler<NormalResultQuery, NormalResultViewModel>,
        IRequestHandler<NormalResultDetailsQuery, NormalDetailViewModel>,
        IRequestHandler<ExcelNormalResultQuery, NormalResultExcelViewModel>
    {
        private readonly INormalResultService _normalResultService;
        private readonly IReportDateService _reportService;
        private readonly ICacheRefService _cacheRefService;

        public NormalResultHandler(
            INormalResultService normalResultService,
            IReportDateService reportService,
            ICacheRefService cacheRefService)
        {
            _normalResultService = normalResultService;
            _reportService = reportService;
            _cacheRefService = cacheRefService;
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Reviewed")]
        public NormalResultViewModel Handle(NormalResultQuery message)
        {
            Check.Argument.IsNotNull(message, "message");
            Check.Argument.IsNotEmpty(message.Language, "message.Language");

            var balanceUpDate = _reportService.GetBalanceUpDate();
            var moveBetsDate = _reportService.GetMoveBetsDate();
            var sports = GetSportsResult(message.ResultDate, message.SportsStatus, moveBetsDate);

            var viewModel = new NormalResultViewModel
            {
                ResultDate = message.ResultDate.Date,
                MinResultDate = balanceUpDate,
                Sports = sports
            };

            if (viewModel.Sports.Any())
            {
                viewModel.SportId = message.SportId > 0 && sports.Any(sport => sport.Value == message.SportId.ToString())
                    ? message.SportId
                    : int.Parse(sports.First().Value);

                viewModel.Leagues = GetLeaguesResult(message.ResultDate, viewModel.SportId, moveBetsDate);

                if (viewModel.Leagues.Any(selectItem => selectItem.Value == message.LeagueId.ToString()))
                {
                    viewModel.LeagueId = message.LeagueId;
                }
            }

            viewModel.HtmlResults = _normalResultService.GetResults(new NormalResultQuery
            {
                ResultDate = viewModel.ResultDate,
                SportId = viewModel.SportId,
                LeagueId = viewModel.LeagueId,
                MoveBetsDate = moveBetsDate
            });

            return viewModel;
        }

        private IEnumerable<SelectListItem> GetSportsResult(DateTime resultDate, IDictionary<int, bool> sportsStatus, DateTime moveBetsDate)
        {
            Check.Argument.IsNotNull(sportsStatus, "sportsStatus");

            var sports = _normalResultService.GetSports(resultDate, sportsStatus, moveBetsDate);

            return sports.Select(sport => new SelectListItem
            {
                Text = _cacheRefService.GetSportName(sport.Id),
                Value = sport.Id.ToString()
            });
        }

        private IEnumerable<SelectListItem> GetLeaguesResult(DateTime resultDate, int sportTypeId, DateTime moveBetsDate)
        {
            var leagues = _normalResultService.GetLeagues(
                                resultDate,
                                sportTypeId,
                                moveBetsDate);

            return leagues.Select(league => new SelectListItem
            {
                Text = _cacheRefService.GetLeagueName(league.Id),
                Value = league.Id.ToString()
            });
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Reviewed")]
        public NormalDetailViewModel Handle(NormalResultDetailsQuery message)
        {
            Check.Argument.IsNotNull(message, "message");

            var sportName = _cacheRefService.GetSportName(message.SportId);

            var leagueName = message.LeagueId > 0
                ? _cacheRefService.GetLeagueName(message.LeagueId)
                : _cacheRefService.GetBetTypeName(message.BetTypeId);

            var viewModel = new NormalDetailViewModel
            {
                Title = $"{sportName} - {leagueName}",
                HtmlResults = _normalResultService.GetResultDetails(message)
            };

            return viewModel;
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Reviewed")]
        public NormalResultExcelViewModel Handle(ExcelNormalResultQuery message)
        {
            Check.Argument.IsNotNull(message, "message");

            var leagueName = message.LeagueId == 0 ? Resources.ShowAllLeaguesExcelTitle : _cacheRefService.GetLeagueName(message.LeagueId);
            var resultDate = message.ResultDate.ToShortDateFormatWithSlash();
            message.MoveBetsDate = _reportService.GetMoveBetsDate();

            message.ExcelTitle = $"{Resources.Results} - {resultDate} | {_cacheRefService.GetSportName(message.SportId)} - {leagueName}";

            var excelResults = _normalResultService.GetExcelResults(message);

            return new NormalResultExcelViewModel
            {
                ExcelResults = excelResults
            };
        }
    }
}
