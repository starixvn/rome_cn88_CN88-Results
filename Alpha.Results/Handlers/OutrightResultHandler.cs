namespace Alpha.Results.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using App_GlobalResources;
    using Fanex.Common.Helper;
    using Fanex.Result.Core.Sportsbook.DTO;
    using MediatR;
    using Models.Requests;
    using Models.Views;
    using Services;
    using Sunplus.Agent.Utilities.Extension;

    public class OutrightResultHandler :
         IRequestHandler<OutrightResultQuery, OutrightResultViewModel>,
         IRequestHandler<ExcelOutrightResultQuery, OutrightResultExcelViewModel>
    {
        private readonly IOutrightResultService _outrightService;
        private readonly IReportDateService _reportService;
        private readonly ICacheRefService _cacheRefService;

        public OutrightResultHandler(
            IOutrightResultService outrightService,
            IReportDateService reportService,
            ICacheRefService cacheRefService)
        {
            _outrightService = outrightService;
            _reportService = reportService;
            _cacheRefService = cacheRefService;
        }

        public OutrightResultExcelViewModel Handle(ExcelOutrightResultQuery message)
        {
            Check.Argument.IsNotNull(message, "message");

            var excelResults = _outrightService.GetExcelResults(message, _reportService.GetBalanceUpDate());

            return new OutrightResultExcelViewModel
            {
                ExcelResults = excelResults.ToArray()
            };
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Reviewed")]
        public OutrightResultViewModel Handle(OutrightResultQuery message)
        {
            Check.Argument.IsNotNull(message, "message");

            DateTime currentDate = DateTime.Today;
            DateTime balanceUpDate = _reportService.GetBalanceUpDate();

            OutrightResultViewModel viewModel = new OutrightResultViewModel
            {
                MinResultDate = balanceUpDate,
                MaxResultDate = currentDate,
                Today = currentDate.ToShortDateFormatWithSlash(),
                Yesterday = currentDate.AddDays(-1).ToShortDateFormatWithSlash(),
                MinResultDateMessage = string.Format(Resources.ViewDataFromDate, balanceUpDate.ToShortDateFormatWithSlash()),
                FromDate = message.FromDate,
                ToDate = message.ToDate
            };

            var outrights = _outrightService.GetOutrights(message.FromDate, message.ToDate, balanceUpDate).OrderBy(outright => outright.SportType);

            if (outrights.Any())
            {
                message.SportType = message.SportType > 0 && outrights.Any(outright => outright.SportType == message.SportType)
                    ? message.SportType
                    : outrights.First().SportType;

                viewModel.SportType = message.SportType;
                if (outrights.Any(outright => outright.LeagueId == message.LeagueId && outright.SportType == viewModel.SportType))
                {
                    viewModel.LeagueId = message.LeagueId;
                }
                else
                {
                    message.LeagueId = 0;
                }

                viewModel = GetSportsAndLeagues(viewModel, outrights, viewModel.SportType, viewModel.LeagueId);
            }

            viewModel.HtmlResults = _outrightService.GetResults(outrights, message);

            return viewModel;
        }

        private OutrightResultViewModel GetSportsAndLeagues(
            OutrightResultViewModel viewModel,
            IEnumerable<Outright> outrights,
            int selectedSportType,
            int selectedLeagueId)
        {
            Check.Argument.IsNotNull(viewModel, "viewModel");
            Check.Argument.IsNotNull(outrights, "outrights");

            bool matchedSportType = false;
            IList<SelectListItem> sportTypes = new List<SelectListItem>();

            var outrightsGroup = outrights.GroupBy(outright => outright.SportType);

            foreach (var group in outrightsGroup)
            {
                var key = group.Key;
                matchedSportType = key == selectedSportType;

                sportTypes.Add(new SelectListItem
                {
                    Value = key.ToString(),
                    Text = _cacheRefService.GetSportName(key),
                    Selected = matchedSportType
                });

                if (matchedSportType)
                {
                    viewModel.Leagues = group
                        .Select(outright => outright.LeagueId)
                        .Distinct()
                        .Select(league => new SelectListItem
                        {
                            Value = league.ToString(),
                            Text = _cacheRefService.GetLeagueName(league),
                            Selected = league == selectedLeagueId
                        });
                }
            }

            viewModel.Sports = sportTypes;

            return viewModel;
        }
    }
}
