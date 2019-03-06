namespace Alpha.Results.Site.Handlers
{
    using System;
    using Constants;
    using MediatR;
    using Models.Requests;
    using Models.Views;

    public class KenoResultHandler :
        IRequestHandler<KenoResultQuery, KenoResultViewModel>
    {
        public KenoResultViewModel Handle(KenoResultQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "KenoResultQuery cannot be null");
            }

            var resultUrl = string.Format(
                "{0}game/keno/kenoresult/resultdetail?date={1}&language={2}&productId={4}&customerid={3}",
                message.RootUrl,
                DateTime.Today.ToString("yyyyMMdd"),
                message.LanguageKey,
                message.CustId,
                ProductIdConstants.Keno);

            var extendTokenUrl = $"{message.RootUrl}game/keno/kenoresult/extendtoken?productId={ProductIdConstants.Keno}&userid={message.CustId}";

            return new KenoResultViewModel
            {
                ResultUrl = resultUrl,
                ExtendTokenUrl = extendTokenUrl
            };
        }
    }
}
