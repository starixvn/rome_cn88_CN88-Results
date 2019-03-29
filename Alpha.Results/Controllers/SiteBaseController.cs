namespace Alpha.Results.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using CN88.Web.Controllers;
    using Sunplus.Agent.Model.Entities;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public abstract class SiteBaseController : CN88BaseController
    {
        protected readonly IUserSession _userSession;

        protected SiteBaseController(IUserSession userSession)
        {
            _userSession = userSession;
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!_userSession.SubPageAccess.ViewTotalBets)
            {
                UnAuthentication(filterContext);
            }
        }
    }
}