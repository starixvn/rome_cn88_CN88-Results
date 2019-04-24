namespace Alpha.Results.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests;
    using Sunplus.Agent.Model.Entities;

    public class KenoController : SiteBaseController
    {
        private readonly IMediator _mediator;

        public KenoController(IMediator mediator, IUserSession userSession)
            : base(userSession)
        {
            _mediator = mediator;
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
