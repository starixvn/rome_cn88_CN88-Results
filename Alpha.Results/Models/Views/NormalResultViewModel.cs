namespace Alpha.Results.Site.Models.Views
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Alpha.Results.Site.Helpers;
    using App_GlobalResources;
    using Utils;

    public class NormalResultViewModel
    {
        public DateTime MinResultDate { get; set; }

        public string MinResultDateMessage
        {
            get
            {
                return string.Format(Resources.ViewDataFromDate, MinResultDate.ToCN88ShortDateFormat());
            }
        }

        public DateTime ResultDate { get; set; }

        public DateTime Yesterday
        {
            get
            {
                return SystemTime.Now.AddDays(-1);
            }
        }

        public DateTime Today
        {
            get
            {
                return SystemTime.Now;
            }
        }

        public IEnumerable<SelectListItem> Sports { get; set; }

        public int SportId { get; set; }

        public IEnumerable<SelectListItem> Leagues { get; set; }

        public int LeagueId { get; set; }

        public string HtmlResults { get; set; }
    }
}