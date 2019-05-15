namespace Alpha.Results.Site.Models.Views
{
    using System;
    using Helpers;

    public class TabViewModel
    {
        private readonly string _selectedTabName;
        private readonly BrowserHelper browserHelper;

        public TabViewModel(
            string selectedTabName,
            bool isEnabledKenoTab,
            bool isEnableNG2,
            bool isEnableLottery,
            bool isEnableBullfighting,
            Func<int, string> getProductName,
            BrowserHelper browserHelper)
        {
            _selectedTabName = selectedTabName;
            IsEnabledKenoTab = isEnabledKenoTab;
            IsEnableNG2 = isEnableNG2;
            IsEnableLottery = isEnableLottery;
            IsEnableBullfighting = isEnableBullfighting;
            GetProductName = getProductName;
            this.browserHelper = browserHelper;
        }

        public string SelectedTabName
        {
            get
            {
                return _selectedTabName.ToLower();
            }
        }

        public bool IsEnabledKenoTab { get; set; }

        public bool IsEnableNG2 { get; }

        public bool IsEnableLottery { get; }

        public bool IsEnableBullfighting { get; }

        public Func<int, string> GetProductName { get; }

        public bool IsDevice => browserHelper.IsMobile() && !browserHelper.IsTablet();
    }
}
