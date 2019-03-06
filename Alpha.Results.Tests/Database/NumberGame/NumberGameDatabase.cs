namespace Alpha.Results.Site.Tests.Database.NumberGame
{
    using System.Data;
    using Helpers;

    public static class NumberGameDatabase
    {
        private static DataSet numberGameResultData = FileHelper.LoadDataSet(@"\Database\NumberGame\NumberGameResult.xml");

        public static DataSet NumberGameResultData => numberGameResultData;
    }
}
