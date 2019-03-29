namespace Alpha.Results.Site.Tests.DataAccesses
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Database.Parlay;
    using Fanex.Data;
    using Fanex.Result.Core.Sportsbook.DTO;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Criteria;
    using Site.DataAccesses.Impl;

    [TestFixture]
    public class ParlayResultDataAccessTests
    {
        private CompareLogic compareLogic;
        private IObjectDb objectDb;
        private ParlayResultDataAccess dataAccess;

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

            dataAccess = new ParlayResultDataAccess();
        }

        [Test]
        public void GetParlayResults_CriteriaIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => dataAccess.GetParlayResults(null)));
        }

        [Test]
        public void GetParlayResults_EmptyDataReturnFromDb_ReturnsNull()
        {
            // Arrange
            var criteria = new GetParlayResultCriteria();

            var dataTable = new DataTable();
            objectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataTable.CreateDataReader());

            var expectedData = new List<Parlay> {
                new Parlay {
                    SportId =0,
                    LeagueId =0,
                    Sportbook = new List<Sportsbook>()
                }
            };

            // Act
            var actualData = dataAccess.GetParlayResults(criteria);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Test]
        public void GetParlayResults_HasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var criteria = new GetParlayResultCriteria();

            var dataReader = ParlayDatabase.ParlayResultData.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataReader);

            var expectedData = new List<Parlay> {
                new Parlay {
                    SportId = 1,
                    LeagueId = 165,
                    Sportbook = new List<Sportsbook>
                    {
                        new Sportsbook
                        {
                            AwayId = 1201,
                            HomeId = 1200,
                            BetStatus = "1FL:1-1;FL:1-1;",
                            LeagueId = 165,
                            MatchId = 70000113,
                            EventStatus = "completed",
                            FinalAwayScore = 1,
                            HtAwayScore = 1,
                            Color = string.Empty,
                            FinalHomeScore = 0,
                            HtHomeScore = 0,
                            EventDate = new DateTime(2017, 11 , 16),
                            IsNeutral = false,
                            KickOffTime =  new DateTime(2017, 11, 16),
                            MatchCode = "QC_NAP_Delete_Parlay",
                            GoalRecord = "A;",
                            IsHTCal = true,
                            Calculate = true,
                            RefNo = string.Empty,
                            GameDetails = new List<GameDetail>(),
                            Bettype = 1,
                            BetID = string.Empty,
                            SettlementStatus = 0
                        },
                    }
                },
                new Parlay
                {
                    SportId = 1,
                    LeagueId = 166,
                    Sportbook = new List<Sportsbook>
                    {
                        new Sportsbook
                        {
                            AwayId = 1201,
                            HomeId = 1200,
                            BetStatus = "1FL:1-1;FL:1-1;",
                            LeagueId = 166,
                            MatchId = 70000112,
                            EventStatus = "completed",
                            FinalAwayScore = 0,
                            HtAwayScore = 0,
                            Color = string.Empty,
                            FinalHomeScore = 1,
                            HtHomeScore = 1,
                            EventDate = new DateTime(2017, 11 , 16),
                            IsNeutral = false,
                            KickOffTime =  new DateTime(2017, 11, 16),
                            MatchCode = "QC_NAP_Delete_Parlay",
                            GoalRecord = "H;",
                            IsHTCal = true,
                            Calculate = true,
                            RefNo = string.Empty,
                            GameDetails = new List<GameDetail>
                            {
                                new GameDetail { MatchId = 70000112, ResourceId = "01", HomeValue = "0", AwayValue = "1", ResultStatusID = 5, SettlementStatusID = 4, OtherInfo = "0:1" }
                            },
                            Bettype = 0,
                            BetID = string.Empty
                        }
                    }
                }
            };

            // Act
            var actualData = dataAccess.GetParlayResults(criteria);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedData, actualData).AreEqual);
        }
    }
}
