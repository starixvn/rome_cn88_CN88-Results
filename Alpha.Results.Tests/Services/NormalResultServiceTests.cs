namespace Alpha.Results.Site.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Constants;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Constants;
    using Site.Criteria;
    using Site.Models;
    using Site.Models.Requests;
    using Site.Services;
    using Site.Services.Impl;

    [TestFixture]
    public class NormalResultServiceTests
    {
        private IDynamicRepository _dynamicRepository;
        private NormalResultService _resultService;
        private IResultServiceFactory _resultServiceFactory;

        [SetUp]
        public void SetUp()
        {
            _dynamicRepository = Substitute.For<IDynamicRepository>();
            _resultServiceFactory = Substitute.For<IResultServiceFactory>();
            _resultService = new NormalResultService(_dynamicRepository, _resultServiceFactory);
        }

        #region Get Leagues

        [TestCase(0)]
        [TestCase(-1)]
        public void GetLeagues_SportIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int sportId)
        {
            Assert.Throws<ArgumentOutOfRangeException>(new TestDelegate(
                () => _resultService.GetLeagues(DateTime.Now, sportId, DateTime.Now)));
        }
      
        [Test]
        public void GetLeagues_ReturnLeagues()
        {
            //// Arrange
            var sportId = 1;
            var resultDate = new DateTime(1989, 5, 28);
            var moveBetsDate = new DateTime(1989, 5, 28);
            var expectedLeague = new List<League>
            {
                new League
                {
                    Id = 1,
                    Name = "League Name"
                }
            };

            _dynamicRepository.Fetch<League>(Arg.Is<GetResultLeaguesByDateCriteria>(criteria =>
                    criteria.EventDate == resultDate.ToShortDateString()
                    && criteria.SportId == sportId)).Returns(expectedLeague);

            //// Act
            var leagues = _resultService.GetLeagues(resultDate, sportId, moveBetsDate);

            //// Assert
            Assert.IsTrue(leagues.Count() == 1
                && leagues.ElementAt(0).Id == 1
                && leagues.ElementAt(0).Name == "League Name");
        }

        #endregion Get League

        #region Get Sports

        [Test]
        public void GetSports_ReturnSports()
        {
            //// Arrange
            var resultDate = new DateTime(1989, 5, 28);
            var moveBetsDate = new DateTime(1989, 5, 28);
            var expectedSports = new List<Sport>
            {
                new Sport
                {
                    Id = 1,
                    Name = "Sport Name"
                }
            };

            _dynamicRepository.Fetch<Sport>(
                Arg.Is<GetResultSportsByDateCriteria>(
                    criteria => criteria.EventDate == resultDate.ToShortDateString()
                        && criteria.MoveBetsDay == moveBetsDate)).Returns(expectedSports);

            //// Act
            var sports = _resultService.GetSports(resultDate, new Dictionary<int, bool>(), moveBetsDate);

            //// Assert
            Assert.IsTrue(sports.Count() == 1
                && sports.ElementAt(0).Id == 1
                && sports.ElementAt(0).Name == "Sport Name");
        }        

        #endregion

        #region Get Results

        [Test]
        public void GetResults_RequestIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => _resultService.GetResults(null)));
        }

        [Test]
        public void GetResults_RequestIsNotNull_CallCreateProductResultService()
        {
            // Arrange
            var request = new NormalResultQuery();

            // Act
            _resultService.GetResults(request);

            // Assert
            _resultServiceFactory.Received().CreateProductResultService(Arg.Is(request.SportId), Arg.Is(request.BetTypeId));
        }

        #endregion

        #region Get Excel Results

        [Test]
        public void GetExcelResults_RequestIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => _resultService.GetExcelResults(null)));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void GetExcelResults_SportIdIsNegativeOrZero_ThrowArgumentOutOfRangeException(int sportId)
        {
            // Arrange
            var request = new ExcelNormalResultQuery
            {
                SportId = sportId
            };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(new TestDelegate(
                () => _resultService.GetExcelResults(request)));
        }

        [TestCase(null)]
        [TestCase("")]
        public void GetExcelResults_TemplatePathIsNullOrEmpty_ThrowArgumentException(string templatePath)
        {
            // Arrange
            var request = new ExcelNormalResultQuery
            {
                SportId = 1,
                TemplatePath = templatePath
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(new TestDelegate(
                () => _resultService.GetExcelResults(request)));
        }

        [Test]
        public void GetExcelResults_RequestIsNotNull_CallCreateProductResultService()
        {
            // Arrange
            var request = new ExcelNormalResultQuery
            {
                SportId = 1,
                TemplatePath = "TemplatePath"
            };
            _resultServiceFactory
                .CreateProductResultService(Arg.Any<int>(), Arg.Any<int>())
                .ExportExcel(Arg.Any<ExcelNormalResultQuery>())
                .Returns(new MemoryStream());

            // Act
            _resultService.GetExcelResults(request);

            // Assert
            _resultServiceFactory.Received().CreateProductResultService(Arg.Is(request.SportId), Arg.Is(request.BetTypeId));
        }

        #endregion

        #region Get Result Details

        [Test]
        public void GetResultDetails_RequestIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => _resultService.GetResultDetails(null)));
        }

        [Test]
        public void GetResultDetails_RequestIsNotNull_CallCreateProductResultService()
        {
            // Arrange
            var request = new NormalResultDetailsQuery();

            // Act
            _resultService.GetResultDetails(request);

            // Assert
            _resultServiceFactory.Received().CreateProductResultService(Arg.Is(request.SportId), Arg.Is(request.BetTypeId));
        }

        #endregion
    }
}
