namespace Alpha.Results.Site.Tests.Handlers
{
    using System;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Handlers;
    using Site.Models.Requests;
    using Site.Models.Views;
    using Site.Services;

    [TestFixture]
    public class PokerGamesHandlerTests
    {
        private PokerGamesHandler handler;
        private ICacheRefService cacheRefService;
        private CompareLogic compareLogic;

        [SetUp]
        public void SetUp()
        {
            cacheRefService = Substitute.For<ICacheRefService>();
            cacheRefService.GetProductName(Arg.Any<int>()).Returns("ProductName");

            handler = new PokerGamesHandler(cacheRefService);
            compareLogic = new CompareLogic();
        }

        [Test]
        public void HandlePokerGamesResultQuery_QueryIsNull_ThrowArgumentNullException()
        {
            PokerGamesResultQuery request = null;

            Assert.Throws<ArgumentNullException>(() => handler.Handle(request));
        }

        [Test]
        public void HandleThirdPartyResultQuery_QueryIsNotNullAndHasException_CallsCreateProductResultService()
        {
            // Arrange
            cacheRefService.GetProductName(Arg.Any<int>()).Returns("ProductName");
            var query = new PokerGamesResultQuery();
            var expectedValue = new PokerGamesViewModel
            {
                Title = "ProductName",
                Result = "Result.Core Exception: custId can not be null or emtpy  ",
                IsSuccess = false
            };

            // Act
            var actualValue = handler.Handle(query);

            // Assert
            Assert.True(compareLogic.Compare(expectedValue, actualValue).AreEqual);
        }
    }
}
