namespace Alpha.Results.Site.DataAccesses
{
    using System.Collections.Generic;
    using Alpha.Results.Site.Criteria;
    using Fanex.Result.Core.NumberGame.DTO;

    /// <summary>
    /// The interface INumberGameResultDataAccess.
    /// </summary>
    public interface INumberGameResultDataAccess
    {
        IList<NumberGame> GetNumberGameResults(GetNumberGameResultCriteria criteria);

        IList<NumberGame> GetNumberGameResultDetails(GetNumberGameResultDetailsCriteria criteria);
    }
}