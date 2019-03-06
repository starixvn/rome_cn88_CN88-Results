namespace Alpha.Results.Site.Tests.Services.Products
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.Sportsbook.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Criteria;
    using Site.DataAccesses;
    using Site.Models.Requests;
    using Site.Services;
    using Site.Services.Impl.Products;

    [TestFixture]
    public class SportsbookResultServiceTests
    {
        private ISportsbookResultDataAccess _sportsbookResultDataAccess;
        private IResultEngineAdapter _resultEngineAdapter;
        private SportsbookResultService _sportsbookResultService;

        [SetUp]
        public void SetUp()
        {
            _sportsbookResultDataAccess = Substitute.For<ISportsbookResultDataAccess>();
            _resultEngineAdapter = Substitute.For<IResultEngineAdapter>();
            _sportsbookResultService = new SportsbookResultService(_sportsbookResultDataAccess, _resultEngineAdapter);
        }

        [Test]
        public void GetResult_RequestIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _sportsbookResultService.GetResult(null));
        }

        [Test]
        public void GetResult_RequestIsNotNull_CallGetNumberGameResults()
        {
            // Arrange
            var request = new NormalResultQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                SportId = 1,
                LeagueId = 1
            };

            // Act
            _sportsbookResultService.GetResult(request);

            // Assert
            _sportsbookResultDataAccess.Received()
                .GetSportbookResults(Arg.Is<GetSportsbookResultCriteria>(
                    c => c.FromDate == request.ResultDate
                        && c.LeagueId == request.LeagueId
                        && c.SportType == request.SportId));
        }

        [Test]
        public void GetResult_RequestIsNotNull_CallHtmlParser()
        {
            // Arrange
            var request = new NormalResultQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                LeagueId = 1,
                SportId = 1
            };

            // Act
            _sportsbookResultService.GetResult(request);

            // Assert
            _resultEngineAdapter.Received().HtmlParser(
                Arg.Any<IList<Sportsbook>>(),
                Arg.Is<ResultSetting>(
                    s => s.SportType == SportType.Soccer
                        && s.LanguageCode == request.Language
                        && s.VisibleDetailedLink
                        && s.VisibleFreeHeader
                        && !s.VisibleMatchCodeIDColumn
                        && s.HighlightWinnerTeam
                        && s.EnableCheckScoreCalculated));
        }

        [Test]
        public void ExportExcel_RequestIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _sportsbookResultService.ExportExcel(null));
        }

        [Test]
        public void ExportExcel_RequestIsNotNull_CallGetResults()
        {
            // Arrange
            var request = new ExcelNormalResultQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                LeagueId = 1,
                SportId = 1
            };

            // Act
            _sportsbookResultService.ExportExcel(request);

            // Assert
            _sportsbookResultDataAccess.Received()
                .GetSportbookResults(Arg.Is<GetSportsbookResultCriteria>(
                    c => c.FromDate == request.ResultDate
                        && c.LeagueId == request.LeagueId
                        && c.SportType == request.SportId));
        }

        [Test]
        public void ExportExcel_RequestIsNotNull_CallExcelParser()
        {
            // Arrange
            var request = new ExcelNormalResultQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                LeagueId = 1,
                SportId = 1,
                ExcelTitle = "ExelTitle",
                TemplatePath = "Path"
            };

            // Act
            _sportsbookResultService.ExportExcel(request);

            // Assert
            _resultEngineAdapter.Received().ExcelParser(
                Arg.Any<IList<Sportsbook>>(),
                Arg.Is<ResultSetting>(
                    resultSetting => resultSetting.SportType == SportType.Soccer
                                    && resultSetting.LanguageCode == request.Language
                                    && resultSetting.ExcelTitle == request.ExcelTitle
                                    && resultSetting.ExcelTemplatePath == request.TemplatePath
                                    && !resultSetting.VisibleMatchCodeIDColumn
                                    && resultSetting.HighlightWinnerTeam
                                    && resultSetting.EnableCheckScoreCalculated));
        }

        [Test]
        public void GetResultDetails_QueryIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _sportsbookResultService.GetResultDetails(null));
        }

        [Test]
        public void GetResultDetails_WhenCalled_ExecuteFetch()
        {
            //// Arrange
            var query = new NormalResultDetailsQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                MatchId = 1,
                BetTypeId = 1,
                SportId = 1,
                BetId = 1,
                IsViewResultFromBetList = true
            };

            //// Act
            _sportsbookResultService.GetResultDetails(query);

            //// Assert
            _sportsbookResultDataAccess.Received().GetScoreInformation(
                Arg.Is<GetSportsbookResultDetailsCriteria>(
                    criteria => criteria.MatchId == query.MatchId
                            && criteria.BetType == query.BetTypeId
                            && criteria.SportType == query.SportId
                            && criteria.BetId == query.BetId
                            && criteria.IsViewResultFromBetList == query.IsViewResultFromBetList));
        }

        [Test]
        public void GetResultDetails_GotDataFromRepository_UpdateDataWithQueryBetType()
        {
            //// Arrange
            var query = new NormalResultDetailsQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                MatchId = 3,
                IsViewResultFromBetList = true
            };

            var results = new List<Sportsbook>
            {
                new Sportsbook()
            };

            _sportsbookResultDataAccess.GetScoreInformation(Arg.Any<GetSportsbookResultDetailsCriteria>()).Returns(results);

            //// Act
            _sportsbookResultService.GetResultDetails(query);

            //// Assert
            _resultEngineAdapter.Received().HtmlParser(
                Arg.Is<List<Sportsbook>>(items => items.Count == 1),
                Arg.Any<ResultSetting>());
        }

        [Test]
        public void GetResultDetails_ViewResultFromBetList_ExecuteHtmlParserWithResultSetting()
        {
            //// Arrange
            var query = new NormalResultDetailsQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                MatchId = 1,
                SportId = 1,
                IsViewResultFromBetList = true
            };

            //// Act
            _sportsbookResultService.GetResultDetails(query);

            //// Assert
            _resultEngineAdapter.Received().HtmlParser(
                Arg.Any<IList<Sportsbook>>(),
                Arg.Is<ResultSetting>(
                resultSetting => resultSetting.SportType == SportType.Soccer
                                && !resultSetting.IsOutright
                                && !resultSetting.IsParlayPopUp
                                && resultSetting.IsPopUp
                                && resultSetting.LanguageCode == query.Language
                                && !resultSetting.VisibleMatchCodeIDColumn
                                && resultSetting.HighlightWinnerTeam
                                && resultSetting.EnableCheckScoreCalculated));
        }

        [Test]
        public void GetResultDetails_ViewResultNotFromBetList_ExecuteHtmlGameDetailParserWithResultSetting()
        {
            //// Arrange
            var query = new NormalResultDetailsQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                MatchId = 1,
                SportId = 1,
                IsViewResultFromBetList = false
            };

            var results = new List<Sportsbook>
            {
                new Sportsbook()
            };

            _sportsbookResultDataAccess.GetScoreInformation(Arg.Any<GetSportsbookResultDetailsCriteria>()).Returns(results);

            //// Act
            _sportsbookResultService.GetResultDetails(query);

            //// Assert
            _resultEngineAdapter.Received().HtmlGameDetailParser(
                Arg.Any<Sportsbook>(),
                Arg.Is<ResultSetting>(
                resultSetting => resultSetting.SportType == SportType.Soccer
                                && !resultSetting.IsOutright
                                && !resultSetting.IsParlayPopUp
                                && resultSetting.IsPopUp
                                && resultSetting.LanguageCode == query.Language
                                && !resultSetting.VisibleMatchCodeIDColumn
                                && resultSetting.HighlightWinnerTeam
                                && resultSetting.EnableCheckScoreCalculated));
        }
    }
}
