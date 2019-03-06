namespace Alpha.Results.Site.App_Start
{
    using System.Web.Optimization;

    public static class BundleConfig
    {
        public const string NormalResultJavascriptBundleUrl = "~/bundles/scripts/normalresult";
        public const string OutrightResultJavascriptBundleUrl = "~/bundles/scripts/outrightresult";
        public const string BullfightingResultJavascriptBundleUrl = "~/bundles/scripts/bullfightingresult";
        public const string ResultCoreStyleBundleUrl = "~/bundle/styles/resultcore";
        public const string KenoResultStyleBundleUrl = "~/bundle/styles/kenoresult";
        public const string ThirdPartyResultStyleBundleUrl = "~/bundle/styles/thirdpartyresult";

        public const string LotteryResultScriptBundleUrl = "~/bundle/styles/lotteryresult";
        public const string PokerGamesResultScriptBundleUrl = "~/bundle/styles/pokergamesresult";

        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterJavascriptBundles(bundles);
            RegisterStyleBundles(bundles);
            RegisterThirdPartyResultStyleBundles(bundles);
        }

        private static void RegisterJavascriptBundles(BundleCollection bundles)
        {
            RegisterNormalResultJavascript(bundles);
            RegisterOutrightResultJavascript(bundles);
            RegisterBullfightingResultJavascript(bundles);
            RegisterKenoResultJavascript(bundles);
            RegisterLotteryResultJavascript(bundles);
            RegisterPokerGamesResultJavascript(bundles);
        }

        private static void RegisterNormalResultJavascript(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(NormalResultJavascriptBundleUrl).Include(
                        "~/Scripts/Normal/normal-result.js"));
        }

        private static void RegisterOutrightResultJavascript(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(OutrightResultJavascriptBundleUrl).Include(
                        "~/Scripts/Outright/outright-result.js"));
        }

        private static void RegisterKenoResultJavascript(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(KenoResultStyleBundleUrl).Include(
                        "~/Scripts/Keno/index.js"));
        }

        private static void RegisterLotteryResultJavascript(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(LotteryResultScriptBundleUrl).Include(
                        "~/Scripts/Lottery/index.js"));

        }

        private static void RegisterPokerGamesResultJavascript(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(PokerGamesResultScriptBundleUrl).Include(
                        "~/Scripts/PokerGames/index.js"));
        }

        private static void RegisterBullfightingResultJavascript(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(BullfightingResultJavascriptBundleUrl).Include("~/Scripts/Bullfighting/index.js"));
        }
        private static void RegisterStyleBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle(ResultCoreStyleBundleUrl)
                .Include("~/Content/Fanex.Result/fanex.result.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/result.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Fanex.Result/Allbet/css/table_w.css", new CssRewriteUrlTransformWrapper()));
        }

        private static void RegisterThirdPartyResultStyleBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle(ThirdPartyResultStyleBundleUrl)
                .Include("~/Content/ThirdParty/third-party.css", new CssRewriteUrlTransformWrapper()));
        }
    }
}
