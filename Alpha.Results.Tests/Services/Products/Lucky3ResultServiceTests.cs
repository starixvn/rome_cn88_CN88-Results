namespace Alpha.Results.Site.Tests.Services.Products
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.Lucky3.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Constants;
    using Site.Criteria;
    using Site.DataAccesses;
    using Site.Models.Requests;
    using Site.Services;
    using Site.Services.Impl.Products;

    [TestFixture]
    public class Lucky3ResultServiceTests
    {
        private Lucky3ResultService lucky3ResultService;
        private ILucky3ResultDataAccess lucky3ResultDataAccess;
        private IResultEngineAdapter resultEngineAdapter;

        [SetUp]
        public void SetUp()
        {
            lucky3ResultDataAccess = Substitute.For<ILucky3ResultDataAccess>();
            resultEngineAdapter = Substitute.For<IResultEngineAdapter>();
            lucky3ResultService = new Lucky3ResultService(lucky3ResultDataAccess, resultEngineAdapter);
        }

        [Test]
        public void GetResultDetails_QueryIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => lucky3ResultService.GetResultDetails(null));
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
            lucky3ResultService.GetResultDetails(query);

            //// Assert
            lucky3ResultDataAccess.Received().GetResultDetails(
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

            var metricGamingTickets = new List<Lucky3>
            {
                new Lucky3()
            };

            lucky3ResultDataAccess.GetResultDetails(Arg.Any<GetNumberGameResultDetailsCriteria>()).Returns(metricGamingTickets);

            //// Act
            lucky3ResultService.GetResultDetails(query);

            //// Assert
            resultEngineAdapter.Received().HtmlParser(
                Arg.Is<List<Lucky3>>(items => items.Count == 1),
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
            lucky3ResultService.GetResultDetails(query);

            //// Assert
            resultEngineAdapter.Received().HtmlParser(
                Arg.Any<IList<Lucky3>>(),
                Arg.Is<ResultSetting>(
                resultSetting => resultSetting.SportType == SportType.Lucky3
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
            Assert.Throws<ArgumentNullException>(() => lucky3ResultService.GetResult(null));
        }

        [Test]
        public void GetResult_RequestIsNotNull_CallGetResultsFromDataAccess()
        {
            // Arrange
            var request = new NormalResultQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                LeagueId = 1,
            };

            // Act
            lucky3ResultService.GetResult(request);

            // Assert
            lucky3ResultDataAccess.Received()
                .GetResults(Arg.Is<GetNumberGameResultCriteria>(
                    c => c.FromDate == request.ResultDate
                    && c.LeagueId == request.LeagueId
                    && c.Language == request.Language
                    && c.SportType == SportTypeIdConstants.Lucky3));
        }

        [Test]
        public void GetResult_RequestIsNotNull_CallHtmlParser()
        {
            // Arrange
            var request = new NormalResultQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                LeagueId = 1,
                SportId = 160
            };

            // Act
            lucky3ResultService.GetResult(request);

            // Assert
            resultEngineAdapter.Received().HtmlParser(
                Arg.Any<IList<Lucky3>>(),
                Arg.Is<ResultSetting>(
                    resultSetting => resultSetting.SportType == SportType.Lucky3
                                    && resultSetting.LanguageCode == request.Language
                                    && resultSetting.VisibleDetailedLink
                                    && resultSetting.VisibleFreeHeader
                                    && !resultSetting.VisibleMatchCodeIDColumn
                                    && resultSetting.HighlightWinnerTeam));
        }

        [Test]
        public void ExportExcel_RequestIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => lucky3ResultService.ExportExcel(null));
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
            lucky3ResultService.ExportExcel(request);

            // Assert
            lucky3ResultDataAccess.Received()
                .GetResults(Arg.Is<GetNumberGameResultCriteria>(
                    c => c.FromDate == request.ResultDate
                        && c.LeagueId == request.LeagueId
                        && c.Language == request.Language
                        && c.SportType == SportTypeIdConstants.Lucky3));
        }

        [Test]
        public void ExportExcel_RequestIsNotNull_CallExcelParser()
        {
            // Arrange
            var request = new ExcelNormalResultQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                LeagueId = 1,
                SportId = 160,
                ExcelTitle = "ExelTitle",
                TemplatePath = "Path"
            };

            // Act
            lucky3ResultService.ExportExcel(request);

            // Assert
            resultEngineAdapter.Received().ExcelParser(
                Arg.Any<IList<Lucky3>>(),
                Arg.Is<ResultSetting>(
                    resultSetting => resultSetting.SportType == SportType.Lucky3
                                    && resultSetting.LanguageCode == request.Language
                                    && resultSetting.ExcelTitle == request.ExcelTitle
                                    && resultSetting.ExcelTemplatePath == request.TemplatePath
                                    && !resultSetting.VisibleMatchCodeIDColumn
                                    && resultSetting.HighlightWinnerTeam));
        }
    }
}
