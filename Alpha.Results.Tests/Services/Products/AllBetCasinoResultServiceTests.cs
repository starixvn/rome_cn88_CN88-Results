namespace Alpha.Results.Site.Tests.Services.Products
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Fanex.Result.Core.AllBetCasino;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Models.Requests;
    using Site.Services.Impl.Products;

    [TestFixture]
    public class AllBetCasinoResultServiceTests
    {
        private IAllbetResultBuilder _allbetResultBuilder;
        private AllBetCasinoResultService _allbetCasinoResultService;

        [SetUp]
        public void SetUp()
        {
            _allbetResultBuilder = Substitute.For<IAllbetResultBuilder>();
            _allbetCasinoResultService = new AllBetCasinoResultService(_allbetResultBuilder);
        }

        [Test]
        public void ExportExcel_ThrowNotImplementedException()
        {
            Assert.Throws<NotImplementedException>(() => _allbetCasinoResultService.ExportExcel(null));
        }

        [Test]
        public void GetResult_ThrowNotImplementedException()
        {
            Assert.Throws<NotImplementedException>(() => _allbetCasinoResultService.GetResult(null));
        }

        [Test]
        public void GetResultDetails_RequestIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => _allbetCasinoResultService.GetResultDetails(null)));
        }

        [Test]
        public void GetResultDetails_ResutIsNotNull_CallAllbetResultBuilderRender()
        {
            // Arrange
            var request = new NormalResultDetailsQuery(new Dictionary<int, bool>(), LanguageCode.English)
            {
                TransDesc = "",
                BetCheck = "",
                BetTypeId = 1
            };

            // Act
            _allbetCasinoResultService.GetResultDetails(request);

            // Assert
            _allbetResultBuilder.Received().Render(
                Arg.Is(request.TransDesc),
                Arg.Is(request.BetCheck),
                Arg.Is(request.BetTypeId.ToString()),
                Arg.Is(request.Language));
        }
    }
}
