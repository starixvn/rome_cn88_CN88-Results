namespace Alpha.Results.Site.Services.Impl.Products
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Fanex.Common.Helper;
    using Fanex.Result.Core.AllBetCasino;
    using Models.Requests;

    public class AllBetCasinoResultService : IProductResultService
    {
        private readonly IAllbetResultBuilder allbetResultBuilder;

        public AllBetCasinoResultService(IAllbetResultBuilder allbetResultBuilder = null)
        {
            this.allbetResultBuilder = allbetResultBuilder ?? new AllbetResultBuilder();
        }

        public MemoryStream ExportExcel(ExcelNormalResultQuery request)
        {
            throw new NotImplementedException();
        }

        public string GetResult(NormalResultQuery request)
        {
            throw new NotImplementedException();
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Reviewed")]
        public string GetResultDetails(NormalResultDetailsQuery detailsRequest)
        {
            Check.Argument.IsNotNull(detailsRequest, "detailsRequest");

            return allbetResultBuilder.Render(detailsRequest.TransDesc, detailsRequest.BetCheck, detailsRequest.BetTypeId.ToString(), detailsRequest.Language);
        }
    }
}
