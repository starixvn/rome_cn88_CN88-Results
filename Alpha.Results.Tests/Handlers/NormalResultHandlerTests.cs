namespace Alpha.Results.Site.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Handlers;
    using Site.Models;
    using Site.Models.Requests;
    using Site.Services;
    using Sunplus.Agent.Utilities.Extension;

    [TestFixture]
    public class NormalResultHandlerTests
    {
        private const string DefaultLanguage = "en-US";

        private INormalResultService _normalResultService;
        private NormalResultHandler _normalResultHandler;
        private IReportDateService _reportDataService;
        private ICacheRefService _cacheRefService;

        [SetUp]
        public void SetUp()
        {
            _normalResultService = Substitute.For<INormalResultService>();
            _reportDataService = Substitute.For<IReportDateService>();
            _cacheRefService = Substitute.For<ICacheRefService>();

            _normalResultHandler = new NormalResultHandler(_normalResultService, _reportDataService, _cacheRefService);
        }

        #region Handle Normal Result Query

        [Test]
        public void HandleNormalResultQuery_QueryIsNull_ThrowArgumentNullException()
        {
            NormalResultQuery query = null;

            Assert.Throws<ArgumentNullException>(() => _normalResultHandler.Handle(query));
        }

        [Test]
        public void HandleNormalResultQuery_WhenCalled_SetResultDateIsToday()
        {
            //// Arrange
            var query = new NormalResultQuery(new Dictionary<int, bool>(),  DefaultLanguage);

            //// Act
            var actualViewModel = _normalResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual(DateTime.Now.Date, actualViewModel.ResultDate.Date);
        }

        [Test]
        public void HandleNormalResultQuery_WhenCalled_ReturnMinResultDateMessage()
        {
            //// Arrange
            var query = new NormalResultQuery(new Dictionary<int, bool>(), DefaultLanguage);

            //// Act
            var actualViewModel = _normalResultHandler.Handle(query);

            //// Assert
            var expectedMinResultDateMessage = $"You can view the report data from {actualViewModel.MinResultDate.ToShortDateFormatWithSlash()}.";
            Assert.AreEqual(expectedMinResultDateMessage, actualViewModel.MinResultDateMessage);
        }

        [Test]
        public void HandleNormalResultQuery_WhenCalled_SetMinResultDateIsBalanceUpDate()
        {
            //// Arrange
            var query = new NormalResultQuery(new Dictionary<int, bool>(), DefaultLanguage);
            var balanceUpDate = new DateTime(2016, 1, 1);

            _reportDataService.GetBalanceUpDate().Returns(balanceUpDate);

            //// Act
            var actualViewModel = _normalResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual(balanceUpDate, actualViewModel.MinResultDate);
        }

        [Test]
        public void HandleNormalResultQuery_SportsAreNotEmpty_ReturnSelectListItemsOfSport()
        {
            //// Arrange
            var query = new NormalResultQuery(new Dictionary<int, bool>(), DefaultLanguage);
            var sports = new List<Sport>
            {
                new Sport { Id = 1 },
                new Sport { Id = 2 }
            };

            _normalResultService.GetSports(Arg.Any<DateTime>(), Arg.Any<IDictionary<int, bool>>(), Arg.Any<DateTime>())
                .Returns(sports);

            //// Act
            var actualViewModel = _normalResultHandler.Handle(query);

            //// Assert
            var actualSelectListItemsOfSport = actualViewModel.Sports;
            Assert.IsTrue(actualSelectListItemsOfSport.ElementAt(0).Value == "1"
                && actualSelectListItemsOfSport.ElementAt(1).Value == "2");
        }

        [Test]
        public void HandleNormalResultQuery_SportsAreNotEmpty_ReturnSportName()
        {
            //// Arrange
            var query = new NormalResultQuery(new Dictionary<int, bool>(), DefaultLanguage);
            var sports = new List<Sport>
            {
                new Sport { Id = 1 }
            };

            _normalResultService.GetSports(Arg.Any<DateTime>(), Arg.Any<IDictionary<int, bool>>(), Arg.Any<DateTime>())
                .Returns(sports);

            var expectedSportName = "sport name";
            _cacheRefService.GetSportName(1).Returns(expectedSportName);

            //// Act
            var actualViewModel = _normalResultHandler.Handle(query);

            //// Assert
            var actualSelectListItemsOfSport = actualViewModel.Sports;
            Assert.AreEqual(expectedSportName, actualSelectListItemsOfSport.ElementAt(0).Text);
        }

        [Test]
        public void HandleNormalResultQuery_SportsStatusIsNull_ThrowArgumentNullException()
        {
            //// Arrange
            var query = new NormalResultQuery(null, DefaultLanguage);

            //// Assert
            Assert.Throws<ArgumentNullException>(() => _normalResultHandler.Handle(query));
        }

        [Test]
        public void HandleNormalResultQuery_AnySportIsSelected_ReturnSelectedSportId()
        {
            //// Arrange
            var query = new NormalResultQuery(new Dictionary<int, bool>(), DefaultLanguage)
            {
                SportId = 1
            };

            var sports = new List<Sport>
            {
                new Sport { Id = 1 }
            };

            _normalResultService.GetSports(Arg.Any<DateTime>(), Arg.Any<IDictionary<int, bool>>(), Arg.Any<DateTime>()).Returns(sports);

            //// Act
            var actualViewModel = _normalResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual(query.SportId, actualViewModel.SportId);
        }

        [Test]
        public void HandleNormalResultQuery_SportSelectedIsNotAvailable_ReturnDefaultSportId()
        {
            //// Arrange
            var query = new NormalResultQuery(new Dictionary<int, bool>(), DefaultLanguage)
            {
                SportId = 10
            };

            var sports = new List<Sport>
            {
                new Sport { Id = 1 }
            };

            _normalResultService.GetSports(Arg.Any<DateTime>(), Arg.Any<IDictionary<int, bool>>(), Arg.Any<DateTime>()).Returns(sports);

            //// Act
            var actualViewModel = _normalResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual(1, actualViewModel.SportId);
        }

        [Test]
        public void HandleNormalResultQuery_NoneSportIsSelected_FirstSportIsSelectedByDefault()
        {
            //// Arrange
            var query = new NormalResultQuery(new Dictionary<int, bool>(), DefaultLanguage)
            {
                SportId = 0
            };

            var sports = new List<Sport>
            {
                new Sport { Id = 1 },
                new Sport { Id = 2 }
            };

            _normalResultService.GetSports(Arg.Any<DateTime>(), Arg.Any<IDictionary<int, bool>>(), Arg.Any<DateTime>()).Returns(sports);

            //// Act
            var actualViewModel = _normalResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual(1, actualViewModel.SportId);
        }

        [Test]
        public void HandleNormalResultQuery_NoSport_NotExecuteGetLeagues()
        {
            //// Arrange
            var query = new NormalResultQuery(new Dictionary<int, bool>(), DefaultLanguage);

            _normalResultService.GetSports(Arg.Any<DateTime>(), Arg.Any<IDictionary<int, bool>>(), Arg.Any<DateTime>())
                .Returns(Enumerable.Empty<Sport>());

            //// Act
            _normalResultHandler.Handle(query);

            //// Assert
            _normalResultService.DidNotReceive().GetLeagues(
                Arg.Any<DateTime>(),
                Arg.Any<int>(),
                Arg.Any<DateTime>());
        }

        [Test]
        public void HandleNormalResultQuery_AnySportIsSelected_ExecuteGetLeagues()
        {
            //// Arrange
            var query = new NormalResultQuery(new Dictionary<int, bool>(), DefaultLanguage);

            _normalResultService.GetSports(Arg.Any<DateTime>(), Arg.Any<IDictionary<int, bool>>(), Arg.Any<DateTime>())
                .Returns(new List<Sport> { new Sport { Id = 1 } });

            //// Act
            _normalResultHandler.Handle(query);

            //// Assert
            _normalResultService.Received().GetLeagues(
                query.ResultDate,
                1,
                Arg.Any<DateTime>());
        }

        [Test]
        public void HandleNormalResultQuery_LeaguesAreNotEmpty_ReturnSelectListItemsOfLeague()
        {
            //// Arrange
            var query = new NormalResultQuery(new Dictionary<int, bool>(), DefaultLanguage);

            _normalResultService.GetSports(Arg.Any<DateTime>(), Arg.Any<IDictionary<int, bool>>(), Arg.Any<DateTime>())
                .Returns(new List<Sport> { new Sport { Id = 1 } });

            _normalResultService.GetLeagues(Arg.Any<DateTime>(), 1, Arg.Any<DateTime>())
                .Returns(new List<League> { new League { Id = 11 } });

            //// Act
            var actualViewModel = _normalResultHandler.Handle(query);

            //// Assert
            Assert.IsTrue(actualViewModel.Leagues.ElementAt(0).Value == "11");
        }

        [Test]
        public void HandleNormalResultQuery_LeaguesAreNotEmpty_ReturnLeagueName()
        {
            //// Arrange
            var query = new NormalResultQuery(new Dictionary<int, bool>(), DefaultLanguage);

            _normalResultService.GetSports(Arg.Any<DateTime>(), Arg.Any<IDictionary<int, bool>>(), Arg.Any<DateTime>())
                .Returns(new List<Sport> { new Sport { Id = 1 } });

            _normalResultService.GetLeagues(Arg.Any<DateTime>(), 1, Arg.Any<DateTime>())
                .Returns(new List<League> { new League { Id = 11 } });

            var expectedLeagueName = "League Name";
            _cacheRefService.GetLeagueName(11).Returns(expectedLeagueName);

            //// Act
            var actualViewModel = _normalResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual(expectedLeagueName, actualViewModel.Leagues.ElementAt(0).Text);
        }

        [Test]
        public void HandleNormalResultQuery_LeaguesContainsLeagueIdInMessage_ReturnLeagueId()
        {
            //// Arrange
            var query = new NormalResultQuery(new Dictionary<int, bool>(), DefaultLanguage);
            query.LeagueId = 11;

            _normalResultService.GetSports(Arg.Any<DateTime>(), Arg.Any<IDictionary<int, bool>>(), Arg.Any<DateTime>())
                .Returns(new List<Sport> { new Sport { Id = 1 } });

            _normalResultService.GetLeagues(Arg.Any<DateTime>(), 1, Arg.Any<DateTime>())
                .Returns(new List<League> { new League { Id = 11 } });

            var expectedLeaugeId = 11;

            //// Act
            var actualViewModel = _normalResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual(expectedLeaugeId, actualViewModel.LeagueId);
        }

        [Test]
        public void HandleNormalResultQuery_WhenCalled_ExecuteGetResults()
        {
            //// Arrange
            var resultQuery = new NormalResultQuery(new Dictionary<int, bool>(), DefaultLanguage)
            {
                SportId = 1,
                LeagueId = 2
            };

            _normalResultService.GetSports(Arg.Any<DateTime>(), Arg.Any<IDictionary<int, bool>>(), Arg.Any<DateTime>())
                .Returns(new List<Sport> { new Sport { Id = 1 } });

            //// Act
            _normalResultHandler.Handle(resultQuery);

            //// Assert
            _normalResultService.Received().GetResults(Arg.Is<NormalResultQuery>(
                query => query.ResultDate.Date == resultQuery.ResultDate.Date
                        && query.SportId == resultQuery.SportId));
        }

        [Test]
        public void HandleNormalResultQuery_WhenCalled_ReturnHtmlResults()
        {
            //// Arrange
            var resultQuery = new NormalResultQuery(new Dictionary<int, bool>(), DefaultLanguage);
            var expectedHtmlResult = "Html Result";
            _normalResultService.GetResults(Arg.Any<NormalResultQuery>()).Returns(expectedHtmlResult);

            //// Act
            var actualResultViewModel = _normalResultHandler.Handle(resultQuery);

            //// Assert
            Assert.AreEqual(expectedHtmlResult, actualResultViewModel.HtmlResults);
        }

        #endregion Handle Normal Result Query

        #region Handle Normal Result Details Query

        [Test]
        public void HandleNormalResultDetailsQuery_QueryIsNull_ThrowArgumentNullException()
        {
            NormalResultDetailsQuery query = null;

            Assert.Throws<ArgumentNullException>(() => _normalResultHandler.Handle(query));
        }

        [Test]
        public void HandleNormalResultDetailsQuery_LeagueIdIsGreaterThanZero_BuildTitleFromSportAndLeague()
        {
            //// Arrange
            var query = new NormalResultDetailsQuery
            {
                SportId = 1,
                LeagueId = 2
            };

            _cacheRefService.GetSportName(1).Returns("Sport Name");
            _cacheRefService.GetLeagueName(2).Returns("League Name");

            //// Act
            var actualViewModel = _normalResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual("Sport Name - League Name", actualViewModel.Title);
        }

        [Test]
        public void HandleNormalResultDetailsQuery_LeagueIdIsZero_BuildTitleFromSportAndBetType()
        {
            //// Arrange
            var query = new NormalResultDetailsQuery
            {
                SportId = 1,
                LeagueId = 0,
                BetTypeId = 4
            };

            _cacheRefService.GetSportName(1).Returns("Sport Name");
            _cacheRefService.GetBetTypeName(4).Returns("Bet Type Name");

            //// Act
            var actualViewModel = _normalResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual("Sport Name - Bet Type Name", actualViewModel.Title);
        }

        [Test]
        public void HandleNormalResultDetailsQuery_WhenCall_ExecuteGetResultDetails()
        {
            //// Arrange
            var query = new NormalResultDetailsQuery();

            //// Act
            _normalResultHandler.Handle(query);

            //// Assert
            _normalResultService.Received().GetResultDetails(query);
        }

        [Test]
        public void HandleNormalResultDetailsQuery_WhenCall_ReturnHtmlResultDetails()
        {
            //// Arrange
            var query = new NormalResultDetailsQuery();
            var expectedHtmlResult = "Result Details";
            _normalResultService.GetResultDetails(query).Returns(expectedHtmlResult);

            //// Act
            var resultDetailsViewModel = _normalResultHandler.Handle(query);

            //// Assert
            Assert.AreEqual(expectedHtmlResult, resultDetailsViewModel.HtmlResults);
        }

        #endregion Handle Normal Result Details Query

        #region Handle Excel Normal Result Query

        [Test]
        public void HandleExcelNormalResultQuery_QueryIsNull_ThrowArgumentNullException()
        {
            ExcelNormalResultQuery query = null;

            Assert.Throws<ArgumentNullException>(() => _normalResultHandler.Handle(query));
        }

        [TestCase(0, "Results - {0} | Sport Name - Show All Leagues")]
        [TestCase(1, "Results - {0} | Sport Name - League Name")]
        public void HandleExcelNormalResultQuery_WhenCalled_BuildExcelTitle(int leagueId, string expectedExcelTitle)
        {
            //// Arrange
            var query = new ExcelNormalResultQuery
            {
                LeagueId = leagueId,
                SportId = 1
            };

            _cacheRefService.GetSportName(1).Returns("Sport Name");
            _cacheRefService.GetLeagueName(leagueId).Returns("League Name");

            //// Act
            _normalResultHandler.Handle(query);

            //// Assert
            _normalResultService.Received().GetExcelResults(Arg.Is<ExcelNormalResultQuery>(
                excelQuery => excelQuery.ExcelTitle == string.Format(expectedExcelTitle, query.ResultDate.ToShortDateFormatWithSlash())));
        }

        [Test]
        public void HandleExcelNormalResultQuery_WhenCalled_ReturnExcelResult()
        {
            //// Arrange
            var query = new ExcelNormalResultQuery();
            var expectedExcelResult = new byte[] { 1, 2, 3 };
            _normalResultService.GetExcelResults(query).Returns(expectedExcelResult);

            //// Act
            var excelViewModel = _normalResultHandler.Handle(query);

            //// Assert
            Assert.IsTrue(
                expectedExcelResult == excelViewModel.ExcelResults
                && excelViewModel.ExcelName == "Results.xls");
        }

        #endregion Handle Excel Normal Result Query
    }
}
