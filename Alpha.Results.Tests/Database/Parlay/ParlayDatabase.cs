namespace Alpha.Results.Site.Tests.Database.Parlay
{
    using System.Data;
    using Helpers;

    public static class ParlayDatabase
    {
        private static DataSet parlayResultData = FileHelper.LoadDataSet(@"\Database\Parlay\ParlayResult.xml");

        public static DataSet ParlayResultData => parlayResultData;
    }
}
