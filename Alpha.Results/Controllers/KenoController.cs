namespace Alpha.Results.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests;
    using Sunplus.Agent.Model.Entities;
    using Web;

    public class KenoController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IUserSession _userSession;

        public KenoController(IMediator mediator, IUserSession userSession)
        {
            _mediator = mediator;
            _userSession = userSession;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = _mediator.Send(new KenoResultQuery
            {
                LanguageKey = _userSession.LangKey,
                CustId = _userSession.CustId,
                CustLevelId = _userSession.RoleId,
                RootUrl = Helpers.UrlHelper.CurrentAppPath
            });

            return View(viewModel);
        }
    }
}
