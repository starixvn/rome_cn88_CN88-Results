namespace Alpha.Results.Site.Tests.Services.Products
{
    using System;
    using System.Collections.Generic;
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
    public class ParlayResultServiceTests
    {
        private const string DefaultLanguage = "en-US";
        private ParlayResultService _parlayResultService;
        private IParlayResultDataAccess _parlayResultDataAccess;
        private IResultEngineAdapter _resultEngineAdapter;

        [SetUp]
        public void SetUp()
        {
            _parlayResultDataAccess = Substitute.For<IParlayResultDataAccess>();
            _resultEngineAdapter = Substitute.For<IResultEngineAdapter>();
            _parlayResultService = new ParlayResultService(_parlayResultDataAccess, _resultEngineAdapter);
        }

        [Test]
        public void GetResultDetails_QueryIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _parlayResultService.GetResultDetails(null));
        }

        [Test]
        public void GetResultDetails_WhenCalled_ExecuteFetch()
        {
            //// Arrange
            var query = new NormalResultDetailsQuery(new Dictionary<int, bool>(), DefaultLanguage)
            {
                RefNum = 2
            };

            //// Act
            _parlayResultService.GetResultDetails(query);

            //// Assert
            _parlayResultDataAccess.Received().GetParlayResults(
                Arg.Is<GetParlayResultCriteria>(criteria => criteria.RefNo == query.RefNum));
        }

        [Test]
        public void GetResultDetails_GotDataFromRepository_UpdateDataWithQueryBetType()
        {
            //// Arrange
            var query = new NormalResultDetailsQuery(new Dictionary<int, bool>(), DefaultLanguage)
            {
                SportId = 1,
                RefNum = 10,
                BetId = 3,
                BetTypeId = 2
            };

            var metricGamingTickets = new List<Parlay>
            {
                new Parlay()
            };

            _parlayResultDataAccess.GetParlayResults(Arg.Any<GetParlayResultCriteria>()).Returns(metricGamingTickets);

            //// Act
            _parlayResultService.GetResultDetails(query);

            //// Assert
            _resultEngineAdapter.Received().HtmlParser(
                Arg.Is<List<Parlay>>(items => items.Count == 1),
                Arg.Any<ResultSetting>());
        }

        [Test]
        public void GetResultDetails_GotDataFromRepository_ExecuteHtmlParserWithResultSetting()
        {
            //// Arrange
            var query = new NormalResultDetailsQuery(new Dictionary<int, bool>(), DefaultLanguage)
            {
                SportId = 1,
                RefNum = 10,
                BetId = 3,
                BetTypeId = 2
            };

            //// Act
            _parlayResultService.GetResultDetails(query);

            //// Assert
            _resultEngineAdapter.Received().HtmlParser(
                Arg.Any<IList<Parlay>>(),
                Arg.Is<ResultSetting>(
                resultSetting => !resultSetting.IsOutright
                                && resultSetting.IsParlayPopUp
                                && resultSetting.IsPopUp
                                && resultSetting.LanguageCode == query.Language
                                && !resultSetting.VisibleMatchCodeIDColumn
                                && resultSetting.VisibleFreeHeader
                                && resultSetting.HighlightWinnerTeam));
        }

        [Test]
        public void GetResult_ThrowNotImplementedException()
        {
            Assert.Throws<NotImplementedException>(() => _parlayResultService.GetResult(null));
        }

        [Test]
        public void ExportExcel_ThrowNotImplementedException()
        {
            Assert.Throws<NotImplementedException>(() => _parlayResultService.ExportExcel(null));
        }
    }
}
