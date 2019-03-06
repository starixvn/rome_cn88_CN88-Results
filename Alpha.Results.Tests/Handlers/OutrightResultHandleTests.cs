namespace Alpha.Results.Site.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Fanex.Result.Core.Sportsbook.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Handlers;
    using Site.Models.Requests;
    using Site.Services;
    using Sunplus.Agent.Utilities.Extension;

    [TestFixture]
    public class OutrightResultHandleTests
    {
        private IOutrightResultService _outrightResultService;
        private OutrightResultHandler _outrightResultHandler;
        private IReportDateService _reportDataService;
        private ICacheRefService _cacheRefService;

        [SetUp]
        public void SetUp()
        {
            _outrightResultService = Substitute.For<IOutrightResultService>();
            _reportDataService = Substitute.For<IReportDateService>();
            _cacheRefService = Substitute.For<ICacheRefService>();

            _outrightResultHandler = new OutrightResultHandler(_outrightResultService, _reportDataService, _cacheRefService);
        }

        #region Outright Result Query

        [Test]
        public void HandleOutrightResultQuery_QueryIsNull_ThrowArgumentNullException()
        {
            OutrightResultQuery query = null;

            Assert.Throws<ArgumentNullException>(() => _outrightResultHandler.Handle(query));
        }

        [Test]
        public void HandleOutrightResultQuery_WhenCalled_SetDefaultFromDateIsToday()
        {
            //// Arrange
            var query = new OutrightResultQuery();

            //// Act
            var actualViewModel = _outrightResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual(DateTime.Now.Date, actualViewModel.FromDate.Date);
        }

        [Test]
        public void HandleOutrightResultQuery_WhenCalled_SetDefaultToDateIsToday()
        {
            //// Arrange
            var query = new OutrightResultQuery();

            //// Act
            var actualViewModel = _outrightResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual(DateTime.Now.Date, actualViewModel.ToDate.Date);
        }

        [Test]
        public void HandleOutrightResultQuery_WhenCalled_ReturnToDayText()
        {
            //// Arrange
            var query = new OutrightResultQuery();

            //// Act
            var actualViewModel = _outrightResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual(actualViewModel.ToDate.ToShortDateFormatWithSlash(), actualViewModel.Today);
        }

        [Test]
        public void HandleOutrightResultQuery_WhenCalled_ReturnYesterdayText()
        {
            //// Arrange
            var query = new OutrightResultQuery();

            //// Act
            var actualViewModel = _outrightResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual(actualViewModel.ToDate.AddDays(-1).ToShortDateFormatWithSlash(), actualViewModel.Yesterday);
        }

        [Test]
        public void HandleOutrightResultQuery_WhenCalled_SetMinResultDateIsBalanceUpDate()
        {
            //// Arrange
            var query = new OutrightResultQuery();
            var balanceUpDate = new DateTime(2016, 1, 1);

            _reportDataService.GetBalanceUpDate().Returns(balanceUpDate);

            //// Act
            var actualViewModel = _outrightResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual(balanceUpDate, actualViewModel.MinResultDate);
        }

        [Test]
        public void HandleOutrightResultQuery_WhenCalled_ReturnMinResultDateMessage()
        {
            //// Arrange
            var query = new OutrightResultQuery();
            var balanceUpDate = new DateTime(2016, 1, 1);

            _reportDataService.GetBalanceUpDate().Returns(balanceUpDate);

            //// Act
            var actualViewModel = _outrightResultHandler.Handle(query);

            //// Assert
            var expectedMinResultDateMessage = $"You can view the report data from {balanceUpDate.ToShortDateFormatWithSlash()}.";
            Assert.AreEqual(expectedMinResultDateMessage, actualViewModel.MinResultDateMessage);
        }

        [Test]
        public void HandleOutrightResultQuery_WhenCalled_SetMaxResultDateIsToday()
        {
            //// Arrange
            var query = new OutrightResultQuery();

            //// Act
            var actualViewModel = _outrightResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual(DateTime.Now.Date, actualViewModel.MaxResultDate);
        }

        [Test]
        public void HandleOutrightResultQuery_WhenCalled_ExecuteGetOutrights()
        {
            //// Arrange
            var query = new OutrightResultQuery();

            var balanceUpDate = new DateTime(2016, 1, 1);
            _reportDataService.GetBalanceUpDate().Returns(balanceUpDate);

            //// Act
            _outrightResultHandler.Handle(query);

            //// Assert
            _outrightResultService.Received().GetOutrights(query.FromDate, query.ToDate, balanceUpDate);
        }

        [Test]
        public void HandleOutrightResultQuery_SportTypeIsSelected_ReturnSelectedSportType()
        {
            //// Arrange
            var query = new OutrightResultQuery
            {
                SportType = 1
            };
            _outrightResultService.GetOutrights(query.FromDate, query.ToDate, Arg.Any<DateTime>())
                .Returns(new List<Outright>
                {
                    new Outright
                    {
                        SportType = 1,
                        LeagueId = 2
                    }
                });

            //// Act
            var actualViewModel = _outrightResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual(1, actualViewModel.SportType);
        }

        [Test]
        public void HandleOutrightResultQuery_NoneOfSportTypeIsSelected_FirstSportTypeIsSelected()
        {
            //// Arrange
            var query = new OutrightResultQuery();
            _outrightResultService.GetOutrights(query.FromDate, query.ToDate, Arg.Any<DateTime>())
                .Returns(new List<Outright>
                {
                    new Outright
                    {
                        SportType = 10,
                        LeagueId = 2
                    }
                });

            //// Act
            var actualViewModel = _outrightResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual(10, actualViewModel.SportType);
        }

        [Test]
        public void HandleOutrightResultQuery_LeagueIsSelected_ReturnSelectedLeague()
        {
            //// Arrange
            var query = new OutrightResultQuery
            {
                LeagueId = 5
            };

            _outrightResultService.GetOutrights(query.FromDate, query.ToDate, Arg.Any<DateTime>())
                .Returns(new List<Outright>
                {
                    new Outright
                    {
                        SportType = 10,
                        LeagueId = 5
                    }
                });

            //// Act
            var actualViewModel = _outrightResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual(5, actualViewModel.LeagueId);
        }

        [Test]
        public void HandleOutrightResultQuery_LeagueIsNotAvailable_ReturnSelectedAllLeague()
        {
            //// Arrange
            var query = new OutrightResultQuery
            {
                LeagueId = 2
            };

            _outrightResultService.GetOutrights(query.FromDate, query.ToDate, Arg.Any<DateTime>())
                .Returns(new List<Outright>
                {
                    new Outright
                    {
                        SportType = 10,
                        LeagueId = 5
                    }
                });

            //// Act
            var actualViewModel = _outrightResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual(0, actualViewModel.LeagueId);
        }

        [Test]
        public void HandleOutrightResultQuery_OutrightsHaveData_ReturnGroupedSports()
        {
            //// Arrange
            var query = new OutrightResultQuery
            {
                SportType = 2
            };

            _outrightResultService.GetOutrights(query.FromDate, query.ToDate, Arg.Any<DateTime>())
                .Returns(new List<Outright>
                {
                    new Outright { SportType = 5, LeagueId = 1 },
                    new Outright { SportType = 2, LeagueId = 2 },
                    new Outright { SportType = 2, LeagueId = 3 }
                });

            //// Act
            var actualViewModel = _outrightResultHandler.Handle(query);

            //// Assert
            Assert.IsTrue(actualViewModel.Sports.Count() == 2
                && actualViewModel.Sports.ElementAt(0).Value == "2"
                && actualViewModel.Sports.ElementAt(1).Value == "5"
                && actualViewModel.Sports.ElementAt(0).Selected);
        }

        [Test]
        public void HandleOutrightResultQuery_OutrightsHaveData_ReturnGroupedLeagues()
        {
            //// Arrange
            var query = new OutrightResultQuery
            {
                SportType = 2,
                LeagueId = 3
            };

            _outrightResultService.GetOutrights(query.FromDate, query.ToDate, Arg.Any<DateTime>())
                .Returns(new List<Outright>
                {
                    new Outright { SportType = 5, LeagueId = 3 },
                    new Outright { SportType = 2, LeagueId = 2 },
                    new Outright { SportType = 2, LeagueId = 3 },
                    new Outright { SportType = 2, LeagueId = 4 }
                });

            //// Act
            var actualViewModel = _outrightResultHandler.Handle(query);

            //// Assert
            Assert.IsTrue(actualViewModel.Leagues.Count() == 3
                && actualViewModel.Leagues.ElementAt(0).Value == "2"
                && actualViewModel.Leagues.ElementAt(1).Value == "3"
                && actualViewModel.Leagues.ElementAt(2).Value == "4"
                && actualViewModel.Leagues.ElementAt(1).Selected);
        }

        [Test]
        public void HandleOutrightResultQuery_OutrightsHaveData_ReturnSportAndLeagueName()
        {
            //// Arrange
            var query = new OutrightResultQuery();

            _outrightResultService.GetOutrights(query.FromDate, query.ToDate, Arg.Any<DateTime>())
                .Returns(new List<Outright>
                {
                    new Outright
                    {
                        SportType = 10,
                        LeagueId = 2
                    }
                });
            var sportName = "Sport Name";
            _cacheRefService.GetSportName(10).Returns(sportName);
            var leagueName = "League Name";
            _cacheRefService.GetLeagueName(2).Returns(leagueName);

            //// Act
            var actualViewModel = _outrightResultHandler.Handle(query);

            //// Assert
            Assert.IsTrue(actualViewModel.Sports.First().Text == sportName
                && actualViewModel.Leagues.First().Text == leagueName);
        }

        [Test]
        public void HandleOutrightResultQuery_OutrightsHaveData_ExecuteGetResults()
        {
            //// Arrange
            var query = new OutrightResultQuery();
            var expectedHtmlResult = "Html Result";
            _outrightResultService.GetResults(Arg.Any<IEnumerable<Outright>>(), Arg.Any<OutrightResultQuery>())
                .Returns(expectedHtmlResult);

            //// Act
            var actualResultViewModel = _outrightResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual(expectedHtmlResult, actualResultViewModel.HtmlResults);
        }

        #endregion Outright Result Query

        #region Outright Excel Result Query

        [Test]
        public void HandleExcelOutrightResultQuery_QueryIsNull_ThrowArgumentNullException()
        {
            ExcelOutrightResultQuery query = null;

            Assert.Throws<ArgumentNullException>(() => _outrightResultHandler.Handle(query));
        }

        [Test]
        public void HandleExcelOutrightResultQuery_WhenCalled_ExecuteGetExcelResults()
        {
            //// Arrange
            var query = new ExcelOutrightResultQuery();

            var balanceUpDate = new DateTime(2016, 1, 1);
            _reportDataService.GetBalanceUpDate().Returns(balanceUpDate);

            var excelResult = new MemoryStream();
            _outrightResultService.GetExcelResults(query, balanceUpDate).Returns(excelResult);

            //// Act
            _outrightResultHandler.Handle(query);

            //// Assert
            _outrightResultService.Received().GetExcelResults(query, balanceUpDate);
        }

        #endregion Outright Excel Result Query
    }
}
