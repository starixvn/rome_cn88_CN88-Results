namespace Alpha.Results.Site.Tests.DataAccesses
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Database.MetricGaming;
    using Fanex.Data;
    using Fanex.Result.Core.MetricGaming.DTO;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Criteria;
    using Site.DataAccesses.Impl;

    [TestFixture]
    public class MetricGamingResultDataAccessTests
    {
        private CompareLogic compareLogic;
        private IObjectDb objectDb;
        private MetricGamingResultDataAccess dataAccess;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            DbSettingProviderManager
                .StartTestingSession()
                .Run();
        }

        [SetUp]
        public void SetUp()
        {
            compareLogic = new CompareLogic();

            objectDb = Substitute.For<IObjectDb>();
            ObjectDbFactory.Mock((key, commandTimeout) => objectDb);

            dataAccess = new MetricGamingResultDataAccess();
        }

        [Test]
        public void GetResults_CriteriaIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => dataAccess.GetResults(null)));
        }

        [Test]
        public void GetResult_EmptyDataReturnFromDb_ReturnsDefaultValue()
        {
            // Arrange
            var criteria = new GetMetricGamingResultCriteria();

            var dataTable = new DataTable();
            objectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataTable.CreateDataReader());

            var expectedData = new List<MetricGaming>();

            // Act
            var actualData = dataAccess.GetResults(criteria);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Test]
        public void GetResult_HasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var criteria = new GetMetricGamingResultCriteria();

            var dataReader = MetricGamingDatabase.MetricGamingResultData.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataReader);

            var expectedData = new List<MetricGaming>
            {
                new MetricGaming
                {
                    LeagueId = 48422,
                    MatchId = 23349167,
                    GameId = 801,
                    BetTypeTime = "52:00-53:00",
                    SettlementStatusId = 0,
                    GameResult = string.Empty,
                    OtherInfo = string.Empty,
                    HomeId = "94",
                    AwayId = "42"
                }
            };

            // Act
            var actualData = dataAccess.GetResults(criteria);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedData, actualData).AreEqual);
        }
    }
}
