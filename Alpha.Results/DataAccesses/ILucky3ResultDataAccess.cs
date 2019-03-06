namespace Alpha.Results.Site.DataAccesses
{
    using Alpha.Results.Site.Criteria;
    using Fanex.Result.Core.Lucky3.DTO;
    using System.Collections.Generic;

    public interface ILucky3ResultDataAccess
    {
        IList<Lucky3> GetResults(GetNumberGameResultCriteria criteria);

        IList<Lucky3> GetResultDetails(GetNumberGameResultDetailsCriteria criteria);
    }
}