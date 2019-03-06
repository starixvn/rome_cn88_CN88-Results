namespace Alpha.Results.Site.Controllers
{
    using System.Web.Mvc;
    using Constants;
    using MediatR;
    using Models.Requests;
    using Sunplus.Agent.Model.Entities;
    using Web;

    public class BullfightingController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IUserSession _userSession;

        public BullfightingController(IMediator mediator, IUserSession userSession)
        {
            _mediator = mediator;
            _userSession = userSession;
        }

        public ActionResult Index()
        {
            var request = new ThirdPartyResultQuery
            {
                CustId = _userSession.CustId,
                Language = _userSession.Language,
                RoleId = _userSession.RoleId,
                ProductId = ProductIdConstants.Bullfighting
            };

            var viewModel = _mediator.Send(request);

            return View(viewModel);
        }
    }
}
