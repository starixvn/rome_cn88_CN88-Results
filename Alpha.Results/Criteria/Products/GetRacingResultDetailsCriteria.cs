namespace Alpha.Results.Site.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// The criteria GetRacingPopupResultCriteria.
    /// </summary>
    public class GetRacingResultDetailsCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "MatchID")]
        public int MatchId { get; set; }

        public int? RaceNo { get; set; }

        public override string GetSettingKey()
        {
            return "Age_BetList_SelMatchResult2";
        }
    }
}