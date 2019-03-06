namespace Alpha.Results.Site.Models.Requests
{
    using MediatR;
    using Views;

    public class PokerGamesResultQuery : IRequest<PokerGamesViewModel>
    {
        public string CustId { get; set; }

        public string GameTypeId { get; set; }

        public string TransId3rd { get; set; }

        public byte RoleId { get; set; }
    }
}