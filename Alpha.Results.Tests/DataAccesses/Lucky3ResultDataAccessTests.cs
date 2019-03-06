namespace Alpha.Results.Site.Tests.DataAccesses
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Database.NumberGame;
    using Fanex.Data;
    using Fanex.Result.Core.Lucky3.DTO;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Criteria;
    using Site.DataAccesses.Impl;

    [TestFixture]
    public class Lucky3ResultDataAccessTests
    {
        private CompareLogic compareLogic;
        private IObjectDb objectDb;
        private Lucky3ResultDataAccess dataAccess;

        [SetUp]
        public void SetUp()
        {
            compareLogic = new CompareLogic();

            objectDb = Substitute.For<IObjectDb>();
            ObjectDbFactory.CreateInstance = (settingKey) => objectDb;

            dataAccess = new Lucky3ResultDataAccess();
        }

        [Test]
        public void GetResults_CriteriaIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => dataAccess.GetResults(null)));
        }

        [Test]
        public void GetResults_EmptyDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var criteria = new GetNumberGameResultCriteria();

            var dataTable = new DataTable();
            objectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataTable.CreateDataReader());

            var expectedData = new List<Lucky3>();

            // Act
            var actualData = dataAccess.GetResults(criteria);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Test]
        public void GetResults_HasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var criteria = new GetNumberGameResultCriteria();

            var dataReader = NumberGameDatabase.NumberGameResultData.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataReader);

            var expectedData = new List<Lucky3>
            {
                new Lucky3
                {
                    LeagueId =  15223,
                    MatchId = 23335912,
                    EventStatus = "Completed",
                    EventDate = new DateTime(2017, 10, 27),
                    KickOffTime = new DateTime(2017, 10, 27),
                    MatchCode = "09230",
                    FinalHomeScore = 69,
                    LiveHomeScore = 69,
                    LiveAwayScore = 0,
                    Seq = new List<int> { 1, 2, 3 },
                    Ball = new List<int> { 34, 23, 69 }
                }
            };

            // Act
            var actualData = dataAccess.GetResults(criteria);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Test]
        public void GetResultDetails_CriteriaIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => dataAccess.GetResultDetails(null)));
        }

        [Test]
        public void GetResultDetails_EmptyDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var criteria = new GetNumberGameResultDetailsCriteria();

            var dataTable = new DataTable();
            objectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataTable.CreateDataReader());

            var expectedData = new List<Lucky3>();

            // Act
            var actualData = dataAccess.GetResultDetails(criteria);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Test]
        public void GetResultDetails_HasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var criteria = new GetNumberGameResultDetailsCriteria();

            var dataReader = NumberGameDatabase.NumberGameResultData.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataReader);

            var expectedData = new List<Lucky3>
            {
                new Lucky3
                {
                    LeagueId =  15223,
                    MatchId = 23335912,
                    EventStatus = "Completed",
                    EventDate = new DateTime(2017, 10, 27),
                    KickOffTime = new DateTime(2017, 10, 27),
                    MatchCode = "09230",
                    FinalHomeScore = 69,
                    LiveHomeScore = 69,
                    LiveAwayScore = 0,
                    Seq = new List<int> { 1, 2, 3 },
                    Ball = new List<int> { 34, 23, 69 }
                }
            };

            // Act
            var actualData = dataAccess.GetResultDetails(criteria);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedData, actualData).AreEqual);
        }
    }
}
