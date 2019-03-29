namespace Alpha.Results.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests;
    using Sunplus.Agent.Model.Entities;

    public class PokerGamesController : SiteBaseController
    {
        private readonly IMediator _mediator;

        public PokerGamesController(IMediator mediator, IUserSession userSession)
            : base(userSession)
        {
            _mediator = mediator;
        }

        public ActionResult Index(PokerGamesResultQuery request)
        {
            request.RoleId = _userSession.RoleId;
            var viewModel = _mediator.Send(request);

            return View(viewModel);
        }
    }
}