namespace Alpha.Results.Site.Controllers
{
    using System.Web.Mvc;
    using CN88.Web.Helpers;
    using Fanex.Products;
    using Models.Views;
    using Services;
    using Sunplus.Agent.Model.Definitions;
    using Sunplus.Agent.Model.Entities.Permission;
    using System.Configuration;

    public class TabController : Controller
    {
        private readonly ICacheRefService cacheRefService;
        private readonly BrowserHelper browserHelper;

        public TabController(ICacheRefService cacheRefService, BrowserHelper browserHelper)
        {
            this.cacheRefService = cacheRefService;
            this.browserHelper = browserHelper;
        }

        [ChildActionOnly]
        public ActionResult Index(string tabName = "Normal")
        {
            var viewModel = new TabViewModel(
                tabName,
                ConfigurationManager.AppSettings["EnableProduct-8"] == "1",
                false,
                ConfigurationManager.AppSettings["EnableProduct-28"] == "1",
                false,
                cacheRefService.GetProductName,
                browserHelper);

            return PartialView(viewModel);
        }
    }
}