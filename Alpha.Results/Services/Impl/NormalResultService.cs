namespace Alpha.Results.Site.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Constants;
    using Criteria;
    using Fanex.Common.Helper;
    using Fanex.Data.Repository;
    using Models;
    using Models.Requests;

    public class NormalResultService : INormalResultService
    {
        private readonly IDynamicRepository _dynamicRepository;
        private readonly IResultServiceFactory _resultServiceFactory;

        public NormalResultService(
            IDynamicRepository dynamicRepository,
            IResultServiceFactory resultServiceFactory)
        {
            _dynamicRepository = dynamicRepository;
            _resultServiceFactory = resultServiceFactory;
        }

        public IEnumerable<League> GetLeagues(DateTime resultDate, int sportId, DateTime moveBetsDate)
        {
            Check.Argument.IsNotNegativeOrZero(sportId, "sportId");

            var leagues = _dynamicRepository.Fetch<League>(new GetResultLeaguesByDateCriteria(moveBetsDate)
            {
                EventDate = resultDate.ToShortDateString(),
                SportId = sportId
            });

            return leagues;
        }

        public IEnumerable<Sport> GetSports(DateTime resultDate, IDictionary<int, bool> sportsStatus, DateTime moveBetsDate)
        {
            var sports = _dynamicRepository.Fetch<Sport>(new GetResultSportsByDateCriteria(moveBetsDate)
            {
                EventDate = resultDate.ToShortDateString()
            }).ToList();

            if (sports.Any())
            {
                sports.RemoveAll(sport => sportsStatus.Keys.Contains(sport.Id) && !sportsStatus[sport.Id]);
            }

            return sports;
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Reviewed")]
        public string GetResults(NormalResultQuery request)
        {
            Check.Argument.IsNotNull(request, "request");

            var resultService = _resultServiceFactory.CreateProductResultService(request.SportId, request.BetTypeId);

            return resultService.GetResult(request);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Reviewed")]
        public byte[] GetExcelResults(ExcelNormalResultQuery request)
        {
            Check.Argument.IsNotNull(request, "request");
            Check.Argument.IsNotNegativeOrZero(request.SportId, "request.SportId");
            Check.Argument.IsNotEmpty(request.TemplatePath, "request.TemplatePath");

            var resultService = _resultServiceFactory.CreateProductResultService(request.SportId, request.BetTypeId);

            var memoryResult = resultService.ExportExcel(request);

            return memoryResult.ToArray();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Reviewed")]
        public string GetResultDetails(NormalResultDetailsQuery detailRequest)
        {
            Check.Argument.IsNotNull(detailRequest, "detailRequest");

            var resultService = _resultServiceFactory.CreateProductResultService(detailRequest.SportId, detailRequest.BetTypeId);

            return resultService.GetResultDetails(detailRequest);
        }
    }
}
