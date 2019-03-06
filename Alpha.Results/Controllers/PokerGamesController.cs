namespace Alpha.Results.Site.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using MediatR;
    using Models;
    using Models.Requests;
    using Services;
    using Sunplus.Agent.Model.Entities;
    using Web;
    using Web.Helpers;

    public class PokerGamesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IUserSession _userSession;
        private readonly IAuthorizationService _authorizationService;

        public PokerGamesController(
            IMediator mediator,
            IUserSession userSession,
            IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _userSession = userSession;
            _authorizationService = authorizationService;
        }

        public ActionResult Index(PokerGamesResultQuery request)
        {
            request.RoleId = _userSession.RoleId;
            var viewModel = _mediator.Send(request);

            return View(viewModel);
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!IsBelonging(filterContext.HttpContext.Request))
            {
                UnAuthorization(filterContext);
            }
        }

        private bool IsBelonging(HttpRequestBase request)
        {
            if (_userSession.IsInternal)
            {
                return true;
            }

            bool isBelonging = false;
            var custIdParam = request["custid"];

            if (!string.IsNullOrWhiteSpace(custIdParam))
            {
                int custId;

                if (int.TryParse(custIdParam, out custId))
                {
                    var actor = new Actor(_userSession.CustId, _userSession.RoleId);
                    isBelonging = actor.IsValid()
                        && _authorizationService.IsBelongingOnPage(actor, custId, _userSession.SubAccId);
                }
            }

            return isBelonging;
        }

        private static void UnAuthorization(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult(MicroSites.OldMainUrl(FormsAuthentication.LoginUrl));

            var session = filterContext.HttpContext.Session;
            session.Clear();
            session.Abandon();

            if (!session.IsCookieless)
            {
                FormsAuthentication.SignOut();
            }
        }
    }
}