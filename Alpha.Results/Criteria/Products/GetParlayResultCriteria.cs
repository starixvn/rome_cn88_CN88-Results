namespace Alpha.Results.Site.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// The criteria GetParlayPopupResultCriteria.
    /// </summary>
    public class GetParlayResultCriteria : CriteriaBase
    {
        public long RefNo { get; set; }

        public override string GetSettingKey()
        {
            return "CS_TicketResult_Parlay";
        }

        public override bool IsValid()
        {
            return RefNo > 0;
        }
    }
}