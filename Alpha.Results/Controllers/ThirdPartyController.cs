namespace Alpha.Results.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Models.Requests;
    using Sunplus.Agent.Model.Entities;

    public class ThirdPartyController : Controller
    {
        private readonly IMediator mediator;
        private readonly IUserSession userSession;

        public ThirdPartyController(IMediator mediator, IUserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
        }

        public ActionResult Index(ThirdPartyResultQuery message)
        {
            message.Language = userSession.Language;

            return View(mediator.Send(message));
        }

        public ActionResult Detail(ThirdPartyDetailResultQuery message)
        {
            message.Language = userSession.Language;

            return View("~/Views/ThirdParty/Index.cshtml", mediator.Send(message));
        }
    }
}
