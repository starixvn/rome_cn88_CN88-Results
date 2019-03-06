namespace Alpha.Results.Site.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using Constants;
    using Fanex.Data.Repository;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.Sportsbook.DTO;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Configurations;
    using Site.Criteria;
    using Site.Models.Requests;
    using Site.Services;
    using Site.Services.Impl;

    [TestFixture]
    public class OutrightResultServiceTests
    {
        private IDynamicRepository _dynamicRepository;
        private IResultEngineAdapter _resultEngineAdatper;
        private OutrightResultService _resultService;
        private CompareLogic compareLogic;
        private ICacheRefService _cacheRefService;

        [SetUp]
        public void SetUp()
        {
            _dynamicRepository = Substitute.For<IDynamicRepository>();
            _resultEngineAdatper = Substitute.For<IResultEngineAdapter>();
            _cacheRefService = Substitute.For<ICacheRefService>();

            _resultService = new OutrightResultService(_dynamicRepository, _resultEngineAdatper, _cacheRefService);

            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["LocalTests"] = "1"
            };

            compareLogic = new CompareLogic();
        }

        #region GetOutrights

        [Test]
        public void GetOutrights_FromDateIsLessThanBalanceUpDate_ReturnEmptyList()
        {
            // Arrange
            var expectedResult = Enumerable.Empty<Outright>();

            // Act
            var actualResult = _resultService.GetOutrights(DateTime.Now, DateTime.Now, DateTime.Now.AddDays(1));

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void GetOutrights_FromDateIsGreaterThanOrEqualBalanceUpDate_CallsFetchFromDynamicRepository()
        {
            // Arrange
            DateTime fromDate;
            DateTime toDate;
            DateTime balanceUpDate;
            fromDate = toDate = balanceUpDate = DateTime.Now;

            var criteria = GetOutrightCriteria(fromDate.Date, toDate.Date.AddDays(1).AddTicks(-1));

            // Act
            _resultService.GetOutrights(fromDate, toDate, balanceUpDate);

            // Assert
            _dynamicRepository.Received().Fetch<Outright>(
                Arg.Is<GetOutRightResultCriteria>(
                    param => param.FromDate == criteria.FromDate
                        && param.ToDate == criteria.ToDate
                        && param.IsShowAll == criteria.IsShowAll));
        }

        #endregion

        #region GetResults

        [Test]
        public void GetResults_OutrightsIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => _resultService.GetResults(null, new OutrightResultQuery())));
        }

        [Test]
        public void GetResults_RequestIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => _resultService.GetResults(new List<Outright>(), null)));
        }

        [Test]
        public void GetResults_RequestIsNotNull_ExecuteHtmlParser()
        {
            // Arrange
            var request = new OutrightResultQuery(LanguageCode.English)
            {
                SportType = 1,
                LeagueId = 1
            };
            var outrights = new List<Outright>();
            var resultSetting = new ResultSetting
            {
                LanguageCode = request.Language,
                IsOutright = true,
                IsPopUp = false,
                VisibleMatchCodeIDColumn = false,
                HighlightWinnerTeam = true
            };

            // Act
            var actualResults = _resultService.GetResults(outrights, request);

            // Assert
            _resultEngineAdatper.Received().HtmlParser
                (Arg.Is<IList<Outright>>(p => compareLogic.Compare(p, outrights).AreEqual),
                Arg.Is<ResultSetting>(setting => compareLogic.Compare(resultSetting, setting).AreEqual));
        }

        #endregion

        [Test]
        public void GetExcelResults_RequestIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => _resultService.GetExcelResults(null, DateTime.Now)));
        }

        [TestCase(null)]
        [TestCase("")]
        public void GetExcelResults_TemplatePathIsNullOrEmpty_ThrowArgumentException(string templatePath)
        {
            Assert.Throws<ArgumentException>(new TestDelegate(
                () => _resultService.GetExcelResults(new ExcelOutrightResultQuery() { TemplatePath = templatePath }, DateTime.Now)));
        }

        [Test]
        public void GeExcelResult_RequestIsNotNull_ExecuteExcelParser()
        {
            // Arrange
            _cacheRefService.GetLeagueName(Arg.Any<int>()).Returns("LeagueName");
            _cacheRefService.GetSportName(Arg.Any<int>()).Returns("SportName");

            var query = new ExcelOutrightResultQuery(LanguageCode.English)
            {
                TemplatePath = "TemplatePath",
                FromDate = new DateTime(2017, 11, 14),
                ToDate = new DateTime(2017, 11, 14),
                SportType = 1,
                LeagueId = 1
            };

            var outrights = new List<Outright>();
            _dynamicRepository.Fetch<Outright>(Arg.Any<GetOutRightResultCriteria>()).Returns(outrights);

            var resultSetting = new ResultSetting
            {
                LanguageCode = query.Language,
                IsOutright = true,
                IsPopUp = false,
                HighlightWinnerTeam = true,
                ExcelTitle = "OUTRIGHT RESULTS - 11/14/2017 -> 11/14/2017 | SportName - LeagueName",
                ExcelTemplatePath = query.TemplatePath
            };

            // Act
            _resultService.GetExcelResults(query, DateTime.Now);

            // Assert
            _resultEngineAdatper.Received().ExcelParser(
                Arg.Is<IList<Outright>>(p => compareLogic.Compare(p, outrights).AreEqual),
                Arg.Is<ResultSetting>(setting => compareLogic.Compare(setting, resultSetting).AreEqual));
        }

        private static GetOutRightResultCriteria GetOutrightCriteria(DateTime fromDate, DateTime toDate)
            => new GetOutRightResultCriteria
            {
                FromDate = fromDate,
                ToDate = toDate,
                IsShowAll = AppSettings.LocalTest
            };
    }
}
