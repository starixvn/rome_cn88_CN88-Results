namespace Alpha.Results.Site.Tests.Services.Products
{
    using System;
    using System.Collections.Generic;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.MetricGaming.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Criteria;
    using Site.DataAccesses;
    using Site.Models.Requests;
    using Site.Services;
    using Site.Services.Impl.Products;

    [TestFixture]
    public class MetricGamingResultServiceTests
    {
        private MetricGamingResultService _metricGamingResultService;
        private IMetricGamingResultDataAccess _metricGamingResultDataAccess;
        private IResultEngineAdapter _resultEngineAdapter;

        [SetUp]
        public void SetUp()
        {
            _metricGamingResultDataAccess = Substitute.For<IMetricGamingResultDataAccess>();
            _resultEngineAdapter = Substitute.For<IResultEngineAdapter>();
            _metricGamingResultService = new MetricGamingResultService(_metricGamingResultDataAccess, _resultEngineAdapter);
        }

        [Test]
        public void GetResultDetails_QueryIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _metricGamingResultService.GetResultDetails(null));
        }

        [Test]
        public void GetResultDetails_WhenCalled_ExecuteFetch()
        {
            //// Arrange
            var query = new NormalResultDetailsQuery
            {
                SportId = 1,
                RefNum = 10,
                BetId = 3,
                BetTypeId = 2
            };

            //// Act
            _metricGamingResultService.GetResultDetails(query);

            //// Assert
            _metricGamingResultDataAccess.Received().GetResults(
                Arg.Is<GetMetricGamingResultCriteria>(criteria => criteria.BetId == query.BetId
                            && criteria.SportType == query.SportId
                            && criteria.BetType == query.BetTypeId
                            && criteria.TransId == query.RefNum));
        }

        [Test]
        public void GetResultDetails_GotDataFromRepository_UpdateDataWithQueryBetType()
        {
            //// Arrange
            var query = new NormalResultDetailsQuery
            {
                SportId = 1,
                RefNum = 10,
                BetId = 3,
                BetTypeId = 2
            };

            var metricGamingTickets = new List<MetricGaming>
            {
                new MetricGaming()
            };

            _metricGamingResultDataAccess.GetResults(Arg.Any<GetMetricGamingResultCriteria>()).Returns(metricGamingTickets);

            //// Act
            _metricGamingResultService.GetResultDetails(query);

            //// Assert
            _resultEngineAdapter.Received().HtmlParser(
                Arg.Is<List<MetricGaming>>(items => items.Count == 1),
                Arg.Any<ResultSetting>());
        }

        [Test]
        public void GetResultDetails_GotDataFromRepository_ExecuteHtmlParserWithResultSetting()
        {
            //// Arrange
            var query = new NormalResultDetailsQuery
            {
                SportId = 1,
                RefNum = 10,
                BetId = 3,
                BetTypeId = 2
            };

            //// Act
            _metricGamingResultService.GetResultDetails(query);

            //// Assert
            _resultEngineAdapter.Received().HtmlParser(
                Arg.Any<IList<MetricGaming>>(),
                Arg.Is<ResultSetting>(
                resultSetting => resultSetting.SportType == SportType.Soccer
                                && !resultSetting.IsOutright
                                && !resultSetting.IsParlayPopUp
                                && resultSetting.IsPopUp
                                && resultSetting.LanguageCode == "en-US"
                                && !resultSetting.VisibleMatchCodeIDColumn
                                && resultSetting.VisiblePopUpTitle
                                && resultSetting.HighlightWinnerTeam));
        }

        [Test]
        public void GetResult_ThrowNotImplementedException()
        {
            Assert.Throws<NotImplementedException>(() => _metricGamingResultService.GetResult(null));
        }

        [Test]
        public void ExportExcel_ThrowNotImplementedException()
        {
            Assert.Throws<NotImplementedException>(() => _metricGamingResultService.ExportExcel(null));
        }
    }
}
