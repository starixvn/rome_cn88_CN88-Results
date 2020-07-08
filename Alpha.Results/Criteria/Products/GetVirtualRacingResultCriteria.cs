namespace Alpha.Results.Site.Criteria
{
    using System;
    using Configurations;
    using Fanex.Data.Repository;

    /// <summary>
    /// The criteria GetVirtualRacingResultCriteria.
    /// </summary>
    public class GetVirtualRacingResultCriteria : CriteriaBase
    {
        public DateTime MoveBetsDay { get; set; }

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

        public override string GetSettingKey()
        {
            return "Age_Result_VSRaceSelByMeeting";
        }

        public override bool IsValid()
        {
            return FromDate > DateTime.MinValue && SportType > 0;
        }
    }
}
