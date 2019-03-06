namespace Alpha.Results.Site.Tests.Services.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Fanex.Data.Repository;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.GoldDeluxe.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Criteria;
    using Site.Models.Requests;
    using Site.Services;
    using Site.Services.Impl.Products;

    [TestFixture]
    public class GoldDeluxeResultServiceTests
    {
        private IDynamicRepository _dynamicRepository;
        private IResultEngineAdapter _resultEngineAdapter;
        private GoldDeluxeResultService _goldDeluxeResultService;

        [SetUp]
        public void SetUp()
        {
            _dynamicRepository = Substitute.For<IDynamicRepository>();
            _resultEngineAdapter = Substitute.For<IResultEngineAdapter>();
            _goldDeluxeResultService = new GoldDeluxeResultService(_dynamicRepository, _resultEngineAdapter);
        }

        [Test]
        public void ExportExcel_ThrowNotImplementedException()
        {
            Assert.Throws<NotImplementedException>(() => _goldDeluxeResultService.ExportExcel(null));
        }

        [Test]
        public void GetResult_ThrowNotImplementedException()
        {
            Assert.Throws<NotImplementedException>(() => _goldDeluxeResultService.GetResult(null));
        }

        [Test]
        public void GetResultDetails_RequestIsNull_ThrowNotImplementedException()
        {
            Assert.Throws<ArgumentNullException>(() => _goldDeluxeResultService.GetResultDetails(null));
        }

        [Test]
        public void GetResultDetails_RequestIsNotNull_CallFetchFromRepository()
        {
            // Arrange
            var request = new NormalResultDetailsQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                RefNum = 1,
                BetTypeId = 1
            };

            // Act
            _goldDeluxeResultService.GetResultDetails(request);

            // Assert
            _dynamicRepository.Received()
                .Fetch<GoldDeluxe>(Arg.Is<GetGoldDeluxeResultCriteria>(
                    c => c.TransId == request.RefNum
                        && c.BetType == request.BetTypeId));
        }

        [Test]
        public void GetResultDetails_GotDataFromRepository_UpdateDataWithQueryBettype()
        {
            // Arrange
            var request = new NormalResultDetailsQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                RefNum = 1,
                BetTypeId = 1
            };

            var goldDeluxeResult = new List<GoldDeluxe>
            {
                new GoldDeluxe { Bettype=10 }
            };

            _dynamicRepository.Fetch<GoldDeluxe>(Arg.Any<GetGoldDeluxeResultCriteria>()).Returns(goldDeluxeResult);

            // Act
            _goldDeluxeResultService.GetResultDetails(request);

            // Assert
            _resultEngineAdapter.Received().HtmlParser(
                Arg.Is<IList<GoldDeluxe>>(items => items.Count == 1 && items.First().Bettype == 1),
                Arg.Any<ResultSetting>());
        }

        [Test]
        public void GetResultDetails_RequestIsNotNull_CallHtmlParser()
        {
            // Arrange
            var request = new NormalResultDetailsQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                RefNum = 1,
                BetTypeId = 1
            };

            // Act
            _goldDeluxeResultService.GetResultDetails(request);

            // Assert
            _resultEngineAdapter.Received().HtmlParser(
                Arg.Any<IList<GoldDeluxe>>(),
                Arg.Is<ResultSetting>(
                    s => s.SportType == SportType.GoldDeluxe
                        && !s.IsOutright
                        && !s.IsParlayPopUp
                        && s.IsPopUp
                        && s.LanguageCode == request.Language
                        && !s.VisibleMatchCodeIDColumn
                        && s.HighlightWinnerTeam));
        }
    }
}
