namespace Alpha.Results.Site.Models.Requests
{
    using System.Diagnostics.CodeAnalysis;
    using Alpha.Results.Site.Models.Views;
    using MediatR;

    public class KenoResultQuery : IRequest<KenoResultViewModel>
    {
        public int CustId { get; set; }

        public int CustLevelId { get; set; }

        public string LanguageKey { get; set; }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Reviewed")]
        public string RootUrl { get; set; }
    }
}
