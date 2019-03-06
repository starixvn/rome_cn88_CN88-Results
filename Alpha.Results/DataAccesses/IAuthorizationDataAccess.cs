namespace Alpha.Results.Site.DataAccesses
{
    using Fanex.Data.Repository;
    using Models;

    public interface IAuthorizationDataAccess
    {
        BelongingInfo IsBelonging(ICriteria criteria);
    }
}
