namespace Alpha.Results.Site.Tests.Services.Products
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.Racing.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Criteria;
    using Site.DataAccesses;
    using Site.Models.Requests;
    using Site.Services;
    using Site.Services.Impl.Products;

    [TestFixture]
    public class VirtualRacingResultServiceTests
    {
        private IVirtualRacingResultDataAccess _racingResultDataAccess;
        private IResultEngineAdapter _resultEngineAdapter;
        private VirtualRacingResultService _vitualRacingResultService;

        [SetUp]
        public void SetUp()
        {
            _racingResultDataAccess = Substitute.For<IVirtualRacingResultDataAccess>();
            _resultEngineAdapter = Substitute.For<IResultEngineAdapter>();
            _vitualRacingResultService = new VirtualRacingResultService(_racingResultDataAccess, _resultEngineAdapter);
        }

        [Test]
        public void GetResult_RequestIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _vitualRacingResultService.GetResult(null));
        }

        [Test]
        public void GetResult_RequestIsNotNull_CallGetNumberGameResults()
        {
            // Arrange
            var request = new NormalResultQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                SportId = 181,
                LeagueId = 1
            };

            // Act
            _vitualRacingResultService.GetResult(request);

            // Assert
            _racingResultDataAccess.Received()
                .GetVirtualRacingResults(Arg.Is<GetVirtualRacingResultCriteria>(
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
                SportId = 181
            };

            // Act
            _vitualRacingResultService.GetResult(request);

            // Assert
            _resultEngineAdapter.Received().HtmlParser(
                Arg.Any<IList<Racing>>(),
                Arg.Is<ResultSetting>(
                    s => s.SportType == SportType.VirtualHorseRacing
                        && s.LanguageCode == request.Language
                        && s.VisibleDetailedLink
                        && s.VisibleFreeHeader
                        && !s.VisibleMatchCodeIDColumn
                        && s.HighlightWinnerTeam));
        }

        [Test]
        public void ExportExcel_RequestIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _vitualRacingResultService.ExportExcel(null));
        }

        [Test]
        public void ExportExcel_RequestIsNotNull_CallGetResults()
        {
            // Arrange
            var request = new ExcelNormalResultQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                LeagueId = 1,
                SportId = 181
            };

            // Act
            _vitualRacingResultService.ExportExcel(request);

            // Assert
            _racingResultDataAccess.Received()
                .GetVirtualRacingResults(Arg.Is<GetVirtualRacingResultCriteria>(
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
                SportId = 181,
                ExcelTitle = "ExelTitle",
                TemplatePath = "Path"
            };

            // Act
            _vitualRacingResultService.ExportExcel(request);

            // Assert
            _resultEngineAdapter.Received().ExcelParser(
                Arg.Any<IList<Racing>>(),
                Arg.Is<ResultSetting>(
                    resultSetting => resultSetting.SportType == SportType.VirtualHorseRacing
                                    && resultSetting.LanguageCode == request.Language
                                    && resultSetting.ExcelTitle == request.ExcelTitle
                                    && resultSetting.ExcelTemplatePath == request.TemplatePath
                                    && !resultSetting.VisibleMatchCodeIDColumn
                                    && resultSetting.HighlightWinnerTeam));
        }

        [Test]
        public void GetResultDetails_QueryIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _vitualRacingResultService.GetResultDetails(null));
        }

        [Test]
        public void GetResultDetails_WhenCalled_ExecuteFetch()
        {
            //// Arrange
            var query = new NormalResultDetailsQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                MatchId = 1,
                Race = "0"
            };

            //// Act
            _vitualRacingResultService.GetResultDetails(query);

            //// Assert
            _racingResultDataAccess.Received().GetVirtualRacingResultDetails(
                Arg.Is<GetRacingResultDetailsCriteria>(
                    criteria => criteria.MatchId == query.MatchId
                            && criteria.RaceNo == 0));
        }

        [Test]
        public void GetResultDetails_GotDataFromRepository_UpdateDataWithQueryBetType()
        {
            //// Arrange
            var query = new NormalResultDetailsQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                MatchId = 3,
                Race = "1"
            };

            var racingTickets = new List<Racing>
            {
                new Racing()
            };

            _racingResultDataAccess.GetVirtualRacingResultDetails(Arg.Any<GetRacingResultDetailsCriteria>()).Returns(racingTickets);

            //// Act
            _vitualRacingResultService.GetResultDetails(query);

            //// Assert
            _resultEngineAdapter.Received().HtmlParser(
                Arg.Is<List<Racing>>(items => items.Count == 1),
                Arg.Any<ResultSetting>());
        }

        [Test]
        public void GetResultDetails_GotDataFromRepository_ExecuteHtmlParserWithResultSetting()
        {
            //// Arrange
            var query = new NormalResultDetailsQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                MatchId = 1,
                Race = "1",
                SportId = 181
            };

            //// Act
            _vitualRacingResultService.GetResultDetails(query);

            //// Assert
            _resultEngineAdapter.Received().HtmlParser(
                Arg.Any<IList<Racing>>(),
                Arg.Is<ResultSetting>(
                resultSetting => resultSetting.SportType == SportType.VirtualHorseRacing
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
    }
}
