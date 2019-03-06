namespace Alpha.Results.Site.Models.Requests
{
    using Fanex.Result.ThirdParty.Models.Requests;
    using Fanex.Result.ThirdParty.Models.Views;
    using MediatR;

    public class ThirdPartyResultQuery : ThirdPartyResultBaseQuery, IRequest<ThirdPartyResultViewModel>
    {
    }
}
