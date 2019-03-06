namespace Alpha.Results.Site.Tests.Database.MetricGaming
{
    using System.Data;
    using Helpers;

    public static class MetricGamingDatabase
    {
        private static DataSet metricGamingResultData = FileHelper.LoadDataSet(@"\Database\MetricGaming\MetricGamingResult.xml");

        public static DataSet MetricGamingResultData => metricGamingResultData;
    }
}
