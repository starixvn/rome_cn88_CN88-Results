namespace Alpha.Results.Site.Models.Views
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class OutrightResultViewModel
    {
        public DateTime MinResultDate { get; set; }

        public DateTime MaxResultDate { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Yesterday { get; set; }

        public string Today { get; set; }

        public string MinResultDateMessage { get; set; }

        public IEnumerable<SelectListItem> Sports { get; set; }

        public int SportType { get; set; }

        public IEnumerable<SelectListItem> Leagues { get; set; }

        public int LeagueId { get; set; }

        public string HtmlResults { get; set; }
    }
}