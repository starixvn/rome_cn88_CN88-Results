namespace Alpha.Results.Site.Controllers
{
    using System.Web.Mvc;
    using CN88.Web.Controllers;
    using MediatR;
    using Models.Requests;
    using Sunplus.Agent.Model.Entities;

    public class PokerGamesController : CN88BaseController
    {
        private readonly IMediator _mediator;
        private readonly IUserSession _userSession;

        public PokerGamesController(
            IMediator mediator,
            IUserSession userSession)
        {
            _mediator = mediator;
            _userSession = userSession;
        }

        public ActionResult Index(PokerGamesResultQuery request)
        {
            request.RoleId = _userSession.RoleId;
            var viewModel = _mediator.Send(request);

            return View(viewModel);
        }
    }
}