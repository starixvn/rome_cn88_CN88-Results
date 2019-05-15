namespace Alpha.Results.Site.Controllers
{
    using System.Web.Mvc;
    using Fanex.Products;
    using Helpers;
    using Models.Views;
    using Services;
    using Sunplus.Agent.Model.Definitions;
    using Sunplus.Agent.Model.Entities.Permission;

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
            var product = ProductManager.GetProduct(ProductIdConstants.NG2);
            var isEnableNG2 = product != null && product.ShowSettings;

            var viewModel = new TabViewModel(
                tabName,
                UserPermission.EnableKeno,
                isEnableNG2,
                true,
                UserPermission.IsEnableProduct(ProductIdConstants.BullFighting),
                cacheRefService.GetProductName,
                browserHelper);

            return PartialView(viewModel);
        }
    }
}
