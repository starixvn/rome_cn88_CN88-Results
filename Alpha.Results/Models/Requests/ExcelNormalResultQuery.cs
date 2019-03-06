namespace Alpha.Results.Site.Models.Requests
{
    using System.Collections.Generic;
    using MediatR;
    using Views;

    public class ExcelNormalResultQuery : NormalResultQuery, IRequest<NormalResultExcelViewModel>
    {
        public ExcelNormalResultQuery()
        {
        }

        internal ExcelNormalResultQuery(
            IDictionary<int, bool> sportStatus,
            string language)
            : base(sportStatus, language)
        {
        }

        public string ExcelTitle { get; set; }

        public string TemplatePath { get; set; }
    }
}
