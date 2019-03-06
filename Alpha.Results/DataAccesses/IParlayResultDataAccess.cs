namespace Alpha.Results.Site.DataAccesses
{
    using System.Collections.Generic;
    using Alpha.Results.Site.Criteria;
    using Fanex.Result.Core.Sportsbook.DTO;

    /// <summary>
    /// The interface IParlayPopupResultDataAccess.
    /// </summary>
    public interface IParlayResultDataAccess
    {
        IList<Parlay> GetParlayResults(GetParlayResultCriteria criteria);
    }
}