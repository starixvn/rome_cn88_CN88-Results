namespace Alpha.Results.Site.Services
{
    using Models;

    public interface IAuthorizationService
    {
        bool IsBelongingOnPage(Actor actor, int custId, int subAccountId, long transId = 0);
    }
}
