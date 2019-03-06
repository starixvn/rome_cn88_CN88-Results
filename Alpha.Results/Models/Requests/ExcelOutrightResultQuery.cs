namespace Alpha.Results.Site.Models.Requests
{
    using Alpha.Results.Site.Models.Views;
    using MediatR;

    public class ExcelOutrightResultQuery : OutrightResultQuery, IRequest<OutrightResultExcelViewModel>
    {
        public ExcelOutrightResultQuery()
        {
        }

        internal ExcelOutrightResultQuery(string language)
            : base(language)
        {
        }

        public string TemplatePath { get; set; }
    }
}
