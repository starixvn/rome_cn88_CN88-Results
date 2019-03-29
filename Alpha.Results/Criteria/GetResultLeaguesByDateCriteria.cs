namespace Alpha.Results.Site.Criteria
{
    using System;
    using Configurations;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetResultLeaguesByDateCriteria : CriteriaBase
    {
        public GetResultLeaguesByDateCriteria(DateTime moveBetsDay)
        {
            MoveBetsDay = moveBetsDay;
        }

        public string EventDate { get; set; }

        [SpParam(SqlParamName = "sporttype")]
        public int SportId { get; set; }

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
            return "Age_Result_LeagueSelByDate";
        }

        public override bool IsValid()
            => SportId > 0;
    }
}
