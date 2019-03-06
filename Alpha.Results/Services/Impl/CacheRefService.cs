namespace Alpha.Results.Site.Services.Impl
{
    using System.Threading;
    using Fanex.RemotingRef;

    public class CacheRefService : ICacheRefService
    {
        private readonly IRemotingRefService remotingRefService;

        public CacheRefService(IRemotingRefService remotingRefService)
        {
            this.remotingRefService = remotingRefService;
        }

        public string GetSportName(int sportTypeId)
            => remotingRefService.GetSportName(sportTypeId.ToString(), LanguageCode);

        public string GetBetTypeName(int betTypeId)
            => remotingRefService.GetBetTypeName(betTypeId.ToString(), LanguageCode);

        public string GetLeagueName(int leagueId)
            => remotingRefService.GetLeagueName(leagueId.ToString(), LanguageCode);

        public string GetProductName(int productId)
            => remotingRefService.GetResourceDataByRefIdAndResourceId(
                "Product",
                productId.ToString(),
                LanguageCode,
                string.Empty);

        private static string LanguageCode
            => Thread.CurrentThread.CurrentCulture.Name;
    }
}