namespace Alpha.Results.Site.Services
{
    using System.IO;
    using Models.Requests;

    public interface IProductResultService
    {
        string GetResult(NormalResultQuery request);

        MemoryStream ExportExcel(ExcelNormalResultQuery request);

        string GetResultDetails(NormalResultDetailsQuery detailsRequest);
    }
}