namespace Alpha.Results.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class NG2Controller : SiteBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/NG2/Index.cshtml", null, $"{Helpers.UrlHelper.NG2ResultPath}/All");
        }

        [HttpGet]
        public ActionResult TicketDetail(int gambleId)
        {
            return RedirectToAction(
                "TicketDetail",
                $"..{Helpers.UrlHelper.NG2ResultPath}",
                new RouteValueDictionary(new
                {
                    gambleId
                }));
        }

        [HttpGet]
        public ActionResult ParlayDetail(string refno)
        {
            return RedirectToAction(
                "ParlayDetail",
                $"..{Helpers.UrlHelper.NG2ResultPath}",
                new RouteValueDictionary(new
                {
                    refno
                }));
        }
    }
}
