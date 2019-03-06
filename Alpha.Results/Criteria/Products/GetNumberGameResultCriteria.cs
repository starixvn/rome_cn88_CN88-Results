namespace Alpha.Results.Site.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// The criteria GetNumberGameResultCriteria.
    /// </summary>
    public class GetNumberGameResultCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public int LeagueId { get; set; }

        public string Language { get; set; }

        public int SportType { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Result_MatchSelNumberGameByLeague";
        }

        public override bool IsValid()
        {
            return FromDate > DateTime.MinValue && LeagueId > 0 && SportType > 0;
        }
    }
}