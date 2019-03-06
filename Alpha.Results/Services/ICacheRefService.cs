namespace Alpha.Results.Site.Services
{
    public interface ICacheRefService
    {
        string GetSportName(int sportTypeId);

        string GetBetTypeName(int betTypeId);

        string GetLeagueName(int leagueId);

        string GetProductName(int productId);
    }
}