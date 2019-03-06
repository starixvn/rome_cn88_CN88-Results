namespace Alpha.Results.Site.Tests.Database.SportsBook
{
    using System.Data;
    using Helpers;

    public static class SportsBookDatabase
    {
        private static DataSet sportsBookResultData = FileHelper.LoadDataSet(@"\Database\SportsBook\SportsBookResult.xml");
        private static DataSet sportsBookResultDetailsData = FileHelper.LoadDataSet(@"\Database\SportsBook\SportsBookResultDetails.xml");

        public static DataSet SportsBookResultData => sportsBookResultData;

        public static DataSet SportsBookResultDetailsData => sportsBookResultDetailsData;
    }
}
