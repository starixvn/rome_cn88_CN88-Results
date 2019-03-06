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
        private readonly ICache _cacheManager;

        public ReportDateService(IDynamicRepository dynamicRepository, ICache cacheManager)
        {
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
            _cacheManager = cacheManager ?? CacheService.CreateInstance(CacheType.MemoryCache);
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
        {
            var data = _cacheManager.Get<BalanceUpAndMoveBetsDate>(BalanceUpAndMoveBetsDateCacheKey);

            if (data == null)
            {
                data = _dynamicRepository.Get<BalanceUpAndMoveBetsDate>(new GetBalanceUpAndMoveBetsDayCriteria());

                if (data == null)
                {
                    throw new Exception("Cannot get balance up date and move bets date.");
                }

                _cacheManager.Set(BalanceUpAndMoveBetsDateCacheKey, data);
            }

            return data;
        }
    }
}