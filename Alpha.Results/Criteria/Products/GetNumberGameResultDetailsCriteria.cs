namespace Alpha.Results.Site.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// The criteria GetNumberGamePopupResultCriteria.
    /// </summary>
    public class GetNumberGameResultDetailsCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_BetList_SelMatchResultNumberGame";
        }

        public override bool IsValid()
        {
            return MatchId > 0;
        }
    }
}