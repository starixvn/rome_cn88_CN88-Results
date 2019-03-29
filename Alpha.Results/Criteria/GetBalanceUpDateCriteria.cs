namespace Alpha.Results.Site.Criteria
{
    using Fanex.Data.Repository;

    public class GetBalanceUpAndMoveBetsDayCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Age_SelBalanceUpDay";
        }

        public override bool IsValid()
            => true;
    }
}