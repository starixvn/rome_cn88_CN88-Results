namespace Alpha.Results.Site.DataAccesses
{
    using System.Collections.Generic;
    using Alpha.Results.Site.Criteria;
    using Fanex.Result.Core.Racing.DTO;

    /// <summary>
    /// The interface IVirtualRacingResultDataAccess.
    /// </summary>
    public interface IVirtualRacingResultDataAccess
    {
        IList<Racing> GetVirtualRacingResults(GetVirtualRacingResultCriteria criteria);

        IList<Racing> GetVirtualRacingResultDetails(GetRacingResultDetailsCriteria criteria);
    }
}