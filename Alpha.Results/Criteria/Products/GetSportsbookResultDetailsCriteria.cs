namespace Alpha.Results.Site.Criteria
{
    using System.Collections;
    using Fanex.Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// The criteria GetSportsBookPopupResultCriteria.
    /// </summary>
    public class GetSportsbookResultDetailsCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public string RaceNo { get; set; }

        [SpParam(Ignored = true)]
        public int SportType { get; set; }

        [SpParam(Ignored = true)]
        public bool IsViewResultFromBetList { get; set; }

        public int BetType { get; set; }

        public long BetId { get; set; }

        [SpParam(Ignored = true)]
        public int Handicap2 { get; set; }

        public override string GetSettingKey()
            => IsViewResultFromBetList
            ? "CS_TicketResult_Match"
            : "Age_Result_MatchSelResultDetail";

        public override Hashtable GetSpParams()
        {
            var inputParams = new Hashtable
            {
                ["matchid"] = MatchId,
            };

            if (IsViewResultFromBetList)
            {
                inputParams.Add("bettype", BetType);
                inputParams.Add("betid", BetId);
            }

            return inputParams;
        }

        public override bool IsValid()
            => MatchId > 0;
    }
}