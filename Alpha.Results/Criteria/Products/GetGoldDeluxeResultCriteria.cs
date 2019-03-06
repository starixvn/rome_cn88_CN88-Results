namespace Alpha.Results.Site.Criteria
{
    using Fanex.Data.Repository;

    public class GetGoldDeluxeResultCriteria : CriteriaBase
    {
        public long TransId { get; set; }

        [SpParam(Ignored = true)]
        public int BetType { get; set; }

        public override string GetSettingKey()
        {
            return "All_BetList_MatchResultGDSel";
        }
    }
}