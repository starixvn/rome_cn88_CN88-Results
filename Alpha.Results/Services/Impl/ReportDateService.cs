namespace Alpha.Results.Site.Services.Impl
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models.Dtos;

    public class ReportDateService : IReportDateService
    {
        private const string BalanceUpAndMoveBetsDateCacheKey = "Alpha.Results.Site.Services.Impl.BalanceUpAndMoveBetsDate";

        private readonly IDynamicRepository _dynamicRepository;

        public ReportDateService(IDynamicRepository dynamicRepository)
        {
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes", Justification = "Reviewed")]
        public DateTime GetBalanceUpDate()
        {
            return GetBalanceUpAndMoveBetsDate().BalanceUpDay;
        }

        public DateTime GetMoveBetsDate()
        {
            return GetBalanceUpAndMoveBetsDate().MoveBetsDay;
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes", Justification = "Reviewed!")]
        public BalanceUpAndMoveBetsDate GetBalanceUpAndMoveBetsDate()
            => ConfiguredCacheService.GetOrSet(
                BalanceUpAndMoveBetsDateCacheKey,
                () =>
                {
                    var data = _dynamicRepository.Get<BalanceUpAndMoveBetsDate>(new GetBalanceUpAndMoveBetsDayCriteria());

                    if (data == null)
                    {
                        throw new Exception("Cannot get balance up date and move bets date.");
                    }

                    return data;
                },
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(120)));
    }
}