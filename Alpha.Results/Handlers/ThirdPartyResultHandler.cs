namespace Alpha.Results.Site.Handlers
{
    using Fanex.Common.Helper;
    using Fanex.Result.ThirdParty.Models.Views;
    using Fanex.Result.ThirdParty.Services;
    using MediatR;
    using Models.Requests;
    using Services;

    public class ThirdPartyResultHandler : IRequestHandler<ThirdPartyResultQuery, ThirdPartyResultViewModel>,
        IRequestHandler<ThirdPartyDetailResultQuery,ThirdPartyResultViewModel>
    {
        private readonly ProductResultServiceFactory serviceFactory;
        private readonly ICacheRefService cacheRefService;

        public ThirdPartyResultHandler(
            ProductResultServiceFactory serviceFactory,
            ICacheRefService cacheRefService)
        {
            this.serviceFactory = serviceFactory;
            this.cacheRefService = cacheRefService;
        }

        public ThirdPartyResultViewModel Handle(ThirdPartyResultQuery message)
        {
            Check.Argument.IsNotNull(message, "message");

            var productName = cacheRefService.GetProductName(message.ProductId);

            var productResultService = serviceFactory.CreateProductResultService(message.ProductId);
            var result = productResultService.GetResultLink(message);

            return new ThirdPartyResultViewModel(productName, result);
        }

        public ThirdPartyResultViewModel Handle(ThirdPartyDetailResultQuery message)
        {
            Check.Argument.IsNotNull(message, "message");

            var productName = cacheRefService.GetProductName(message.ProductId);

            var productResultService = serviceFactory.CreateProductResultService(message.ProductId);
            var result = productResultService.GetDetailLink(message);

            return new ThirdPartyResultViewModel(productName, result);
        }
    }
}
