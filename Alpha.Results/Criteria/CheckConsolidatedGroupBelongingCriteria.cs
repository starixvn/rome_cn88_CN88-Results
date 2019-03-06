namespace Alpha.Results.Site.Criteria
{
    public class CheckConsolidatedGroupBelongingCriteria : CheckBelongingCriteria
    {
        public CheckConsolidatedGroupBelongingCriteria(
            int actorId,
            int custId,
            int subAccountId,
            long transId) : base(actorId, custId, subAccountId, transId)
        {
        }

        public override string GetSettingKey()
            => "Age_Rpt_CustomerConsolidatedGroup_CheckBelonging";
    }
}