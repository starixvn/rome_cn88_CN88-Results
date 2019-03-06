namespace Alpha.Results.Site.Models.Views
{
    public class NormalResultExcelViewModel
    {
        public byte[] ExcelResults { get; set; }

        public string ExcelName
        {
            get
            {
                return "Results.xls";
            }
        }
    }
}