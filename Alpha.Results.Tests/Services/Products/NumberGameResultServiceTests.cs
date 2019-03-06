namespace Alpha.Results.Site.Tests.Services.Products
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.NumberGame.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Constants;
    using Site.Criteria;
    using Site.DataAccesses;
    using Site.Models.Requests;
    using Site.Services;
    using Site.Services.Impl.Products;

    [TestFixture]
    public class NumberGameResultServiceTests
    {
        private NumberGameResultService _numberGamingResultService;
        private INumberGameResultDataAccess _numberGameResultDataAccess;
        private IResultEngineAdapter _resultEngineAdapter;

        [SetUp]
        public void SetUp()
        {
            _numberGameResultDataAccess = Substitute.For<INumberGameResultDataAccess>();
            _resultEngineAdapter = Substitute.For<IResultEngineAdapter>();
            _numberGamingResultService = new NumberGameResultService(_numberGameResultDataAccess, _resultEngineAdapter);
        }

        [Test]
        public void GetResultDetails_QueryIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _numberGamingResultService.GetResultDetails(null));
        }

        [Test]
        public void GetResultDetails_WhenCalled_ExecuteFetch()
        {
            //// Arrange
            var query = new NormalResultDetailsQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                MatchId = 1
            };

            //// Act
            _numberGamingResultService.GetResultDetails(query);

            //// Assert
            _numberGameResultDataAccess.Received().GetNumberGameResultDetails(
                Arg.Is<GetNumberGameResultDetailsCriteria>(criteria => criteria.MatchId == query.MatchId));
        }

        [Test]
        public void GetResultDetails_GotDataFromRepository_UpdateDataWithQueryBetType()
        {
            //// Arrange
            var query = new NormalResultDetailsQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                MatchId = 3
            };

            var metricGamingTickets = new List<NumberGame>
            {
                new NumberGame()
            };

            _numberGameResultDataAccess.GetNumberGameResultDetails(Arg.Any<GetNumberGameResultDetailsCriteria>()).Returns(metricGamingTickets);

            //// Act
            _numberGamingResultService.GetResultDetails(query);

            //// Assert
            _resultEngineAdapter.Received().HtmlParser(
                Arg.Is<List<NumberGame>>(items => items.Count == 1),
                Arg.Any<ResultSetting>());
        }

        [Test]
        public void GetResultDetails_GotDataFromRepository_ExecuteHtmlParserWithResultSetting()
        {
            //// Arrange
            var query = new NormalResultDetailsQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                MatchId = 1
            };

            //// Act
            _numberGamingResultService.GetResultDetails(query);

            //// Assert
            _resultEngineAdapter.Received().HtmlParser(
                Arg.Any<IList<NumberGame>>(),
                Arg.Is<ResultSetting>(
                resultSetting => resultSetting.SportType == SportType.NumberGame
                                && !resultSetting.IsOutright
                                && !resultSetting.IsParlayPopUp
                                && resultSetting.IsPopUp
                                && !resultSetting.VisiblePopUpTitle
                                && resultSetting.LanguageCode == query.Language
                                && !resultSetting.VisibleMatchCodeIDColumn
                                && resultSetting.VisibleFreeHeader
                                && resultSetting.VisibleDetailedLink
                                && resultSetting.HighlightWinnerTeam));
        }

        [Test]
        public void GetResult_RequestIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _numberGamingResultService.GetResult(null));
        }

        [Test]
        public void GetResult_RequestIsNotNull_CallGetNumberGameResults()
        {
            // Arrange
            var request = new NormalResultQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                LeagueId = 1
            };

            // Act
            _numberGamingResultService.GetResult(request);

            // Assert
            _numberGameResultDataAccess.Received()
                .GetNumberGameResults(Arg.Is<GetNumberGameResultCriteria>(
                    c => c.FromDate == request.ResultDate
                        && c.LeagueId == request.LeagueId
                        && c.Language == request.Language
                        && c.SportType == SportTypeIdConstants.NumberGame));
        }

        [Test]
        public void GetResult_RequestIsNotNull_CallHtmlParser()
        {
            // Arrange
            var request = new NormalResultQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                LeagueId = 1,
                SportId = 161
            };

            // Act
            _numberGamingResultService.GetResult(request);

            // Assert
            _resultEngineAdapter.Received().HtmlParser(
                Arg.Any<IList<NumberGame>>(),
                Arg.Is<ResultSetting>(
                    s => s.SportType == SportType.NumberGame
                        && s.LanguageCode == request.Language
                        && s.VisibleDetailedLink
                        && s.VisibleFreeHeader
                        && !s.VisibleMatchCodeIDColumn
                        && s.HighlightWinnerTeam));
        }

        [Test]
        public void ExportExcel_RequestIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _numberGamingResultService.ExportExcel(null));
        }

        [Test]
        public void ExportExcel_RequestIsNotNull_CallGetResults()
        {
            // Arrange
            var request = new ExcelNormalResultQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                LeagueId = 1
            };

            // Act
            _numberGamingResultService.ExportExcel(request);

            // Assert
            _numberGameResultDataAccess.Received()
                .GetNumberGameResults(Arg.Is<GetNumberGameResultCriteria>(
                    c => c.FromDate == request.ResultDate
                        && c.LeagueId == request.LeagueId
                        && c.Language == request.Language
                        && c.SportType == SportTypeIdConstants.NumberGame));
        }

        [Test]
        public void ExportExcel_RequestIsNotNull_CallExcelParser()
        {
            // Arrange
            var request = new ExcelNormalResultQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                LeagueId = 1,
                SportId = 161,
                ExcelTitle = "ExelTitle",
                TemplatePath = "Path"
            };

            // Act
            _numberGamingResultService.ExportExcel(request);

            // Assert
            _resultEngineAdapter.Received().ExcelParser(
                Arg.Any<IList<NumberGame>>(),
                Arg.Is<ResultSetting>(
                    resultSetting => resultSetting.SportType == SportType.NumberGame
                                    && resultSetting.LanguageCode == request.Language
                                    && resultSetting.ExcelTitle == request.ExcelTitle
                                    && resultSetting.ExcelTemplatePath == request.TemplatePath
                                    && !resultSetting.VisibleMatchCodeIDColumn
                                    && resultSetting.HighlightWinnerTeam));
        }
    }
}
