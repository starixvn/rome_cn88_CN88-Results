namespace Alpha.Results.Site.Controllers
{
    using System.Globalization;
    using System.Net;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Routing;
    using App_GlobalResources;
    using Configurations;
    using Sunplus.Agent.Model.Entities;
    using Web.Helpers;

    public abstract class SiteBaseController: Controller
    {
        private const string English = "en-US";
        private readonly CultureInfo defaultCulture = CultureInfo.CreateSpecificCulture(English);

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var user = filterContext.HttpContext?.User;
            var isAuthenticated = AppSettings.IsIntegrationTest ||
                (user != null && user.Identity.IsAuthenticated && GetUserSession().UserId > 0);

            if (!isAuthenticated)
            {
                UnAuthentication(filterContext);
            }
        }

        protected override void Initialize(RequestContext requestContext)
        {
            var userSession = GetUserSession();
            var cultureName = userSession.Language;

            if (string.IsNullOrEmpty(cultureName))
            {
                cultureName = English;
                userSession.Language = cultureName;
            }

            // Modify current thread's cultures
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            Thread.CurrentThread.CurrentCulture.DateTimeFormat = defaultCulture.DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = defaultCulture.NumberFormat;

            base.Initialize(requestContext);
        }

        private static IUserSession GetUserSession()
            => UserSession.Current;

        private static void UnAuthentication(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var response = filterContext.HttpContext.Response;
                response.Clear();
                response.Write("{ \"message\": \"" + Resources.AccessDeny + "\" }");

                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);

                response.TrySkipIisCustomErrors = true;
                response.End();
            }
            else
            {
                filterContext.Result = new RedirectResult(MicroSites.MainUrl("~/auth/index"));
            }
        }
    }
}