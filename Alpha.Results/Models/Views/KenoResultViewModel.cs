namespace Alpha.Results.Site.Models.Views
{
    using System.Diagnostics.CodeAnalysis;

    public class KenoResultViewModel
    {
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Reviewed")]
        public string ResultUrl { get; set; }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Reviewed")]
        public string ExtendTokenUrl { get; set; }
    }
}