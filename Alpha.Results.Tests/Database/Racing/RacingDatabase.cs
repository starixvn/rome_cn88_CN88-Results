namespace Alpha.Results.Site.Tests.Database.Racing
{
    using System.Data;
    using Helpers;

    public static class RacingDatabase
    {
        private static DataSet racingResultData = FileHelper.LoadDataSet(@"\Database\Racing\RacingResult.xml");

        public static DataSet RacingResultData => racingResultData;
    }
}
