namespace Alpha.Results.Site.Tests.DataAccesses
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Database.Racing;
    using Fanex.Data;
    using Fanex.Result.Core.Racing.DTO;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Criteria;
    using Site.DataAccesses.Impl;

    [TestFixture]
    public class VirtualRacingResultDataAccessTests
    {
        private CompareLogic compareLogic;
        private IObjectDb objectDb;
        private VirtualRacingResultDataAccess dataAccess;

        [SetUp]
        public void SetUp()
        {
            compareLogic = new CompareLogic();

            objectDb = Substitute.For<IObjectDb>();
            ObjectDbFactory.CreateInstance = (settingKey) => objectDb;

            dataAccess = new VirtualRacingResultDataAccess();
        }

        [Test]
        public void GetVirtualRacingResults_CriteriaIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => dataAccess.GetVirtualRacingResults(null)));
        }

        [Test]
        public void GetVirtualRacingResults_EmptyDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var criteria = new GetVirtualRacingResultCriteria();

            var dataTable = new DataTable();
            objectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataTable.CreateDataReader());

            var expectedData = new List<Racing>();

            // Act
            var actualData = dataAccess.GetVirtualRacingResults(criteria);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Test]
        public void GetVirtualRacingResults_HasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var criteria = new GetVirtualRacingResultCriteria();

            var dataReader = RacingDatabase.RacingResultData.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataReader);

            var expectedData = new List<Racing>
            {
                new Racing { LeagueId = 27538, EngineId = "226628", EngineNo = "3", IsFavor = false, IsSCR = false, Place = 1, PlaceOdds = 0, WinOdds = 4.30m, RaceName = 1, KickOffTime = new DateTime(2017, 10, 27), Favourite = "F", RowCount = 3, SCROfMatch = "17,15,16", TFOfMatch = string.Empty },
                new Racing { LeagueId = 27538, EngineId = "227039", EngineNo = "7", IsFavor = false, IsSCR = false, Place = 2, PlaceOdds = 0, WinOdds = 12.20m, RaceName = 0, KickOffTime = new DateTime(2017, 10, 27), Favourite = string.Empty, RowCount = 3, SCROfMatch = string.Empty,TFOfMatch = string.Empty },
                new Racing { LeagueId = 27538, EngineId = "226690", EngineNo = "4", IsFavor = false, IsSCR = false, Place = 3, PlaceOdds = 0, WinOdds = 5.40m, RaceName = 0, KickOffTime = new DateTime(2017, 10, 27), Favourite = string.Empty, RowCount = 3, SCROfMatch = string.Empty,TFOfMatch = string.Empty }
            };

            // Act
            var actualData = dataAccess.GetVirtualRacingResults(criteria);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Test]
        public void GetVirtualRacingResultDetails_CriteriaIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => dataAccess.GetVirtualRacingResultDetails(null)));
        }

        [Test]
        public void GetVirtualRacingResultDetails_EmptyDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var criteria = new GetRacingResultDetailsCriteria();

            var dataTable = new DataTable();
            objectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataTable.CreateDataReader());

            var expectedData = new List<Racing>();

            // Act
            var actualData = dataAccess.GetVirtualRacingResultDetails(criteria);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Test]
        public void GetVirtualRacingResultDetails_HasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var criteria = new GetRacingResultDetailsCriteria();

            var dataReader = RacingDatabase.RacingResultData.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataReader);

            var expectedData = new List<Racing>
            {
                new Racing { LeagueId = 27538, EngineId = "226628", EngineNo = "3", IsFavor = false, IsSCR = false, Place = 1, PlaceOdds = 0, WinOdds = 4.30m, RaceName = 1, KickOffTime = new DateTime(2017, 10, 27), Favourite = "F", RowCount = 3, SCROfMatch = "17,15,16", TFOfMatch = string.Empty },
                new Racing { LeagueId = 27538, EngineId = "227039", EngineNo = "7", IsFavor = false, IsSCR = false, Place = 2, PlaceOdds = 0, WinOdds = 12.20m, RaceName = 0, KickOffTime = new DateTime(2017, 10, 27), Favourite = string.Empty, RowCount = 3, SCROfMatch = string.Empty,TFOfMatch = string.Empty },
                new Racing { LeagueId = 27538, EngineId = "226690", EngineNo = "4", IsFavor = false, IsSCR = false, Place = 3, PlaceOdds = 0, WinOdds = 5.40m, RaceName = 0, KickOffTime = new DateTime(2017, 10, 27), Favourite = string.Empty, RowCount = 3, SCROfMatch = string.Empty,TFOfMatch = string.Empty }
            };

            // Act
            var actualData = dataAccess.GetVirtualRacingResultDetails(criteria);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedData, actualData).AreEqual);
        }
    }
}
