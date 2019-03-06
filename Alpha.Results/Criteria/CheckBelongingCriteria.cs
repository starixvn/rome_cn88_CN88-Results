namespace Alpha.Results.Site.Criteria
{
    using Fanex.Data.Repository;

    public class CheckBelongingCriteria : CriteriaBase
    {
        public CheckBelongingCriteria(int actorId, int custId, int subAccountId, long transId)
        {
            ActorId = actorId;
            CustId = custId;
            SubAccountId = subAccountId;
            TransId = transId;
        }

        [SpParam(SqlParamName = "SessionCustId")]
        public int ActorId { get; }

        public int CustId { get; }

        public int SubAccountId { get; }

        public long TransId { get; }

        public bool IsActive { get; }

        public byte RoleId { get; }

        public override string GetSettingKey()
            => "Age_CustomerSelCheckBelonging";

        public override bool IsValid()
            => ActorId > 0 && CustId > 0 && SubAccountId >= 0;
    }
}