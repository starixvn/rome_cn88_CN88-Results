namespace Alpha.Results.Site.Services
{
    public interface IResultServiceFactory
    {
        IProductResultService CreateProductResultService(int sportId, int betTypeId);
    }
}