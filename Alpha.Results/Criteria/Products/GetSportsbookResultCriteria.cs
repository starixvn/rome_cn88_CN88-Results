namespace Alpha.Results.Site.Criteria
{
    using System;
    using Configurations;
    using Fanex.Data.Repository;

    /// <summary>
    /// The criteria GetSportsBookResultCriteria.
    /// </summary>
    public class GetSportsbookResultCriteria : CriteriaBase
    {
        public GetSportsbookResultCriteria(DateTime moveBetsDay)
        {
            MoveBetsDay = moveBetsDay;
        }

        public DateTime FromDate { get; set; }

        public int LeagueId { get; set; }

        public int SportType { get; set; }

        public bool IsShowAll
        {
            get
            {
                return AppSettings.LocalTest;
            }
        }

        public DateTime MoveBetsDay { get; }

        public override string GetSettingKey()
        {
            return "Age_Result_MatchSelByLeague";
        }

        public override bool IsValid()
        {
            return FromDate > DateTime.MinValue && SportType
                > 0 && LeagueId > 0;
        }
    }
}
