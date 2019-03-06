namespace Alpha.Results.Site.Tests.Handlers
{
    using System;
    using Fanex.Result.ThirdParty.Services;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Handlers;
    using Site.Models.Requests;
    using Site.Services;

    [TestFixture]
    public class ThirdPartyResultHandlerTests
    {
        private ProductResultServiceFactory serviceFactory;
        private ThirdPartyResultHandler handler;
        private ICacheRefService cacheRefService;

        [SetUp]
        public void SetUp()
        {
            serviceFactory = Substitute.For<ProductResultServiceFactory>();
            cacheRefService = Substitute.For<ICacheRefService>();

            handler = new ThirdPartyResultHandler(serviceFactory, cacheRefService);
        }

        [Test]
        public void HandleThirdPartyResultQuery_QueryIsNull_ThrowArgumentNullException()
        {
            ThirdPartyResultQuery request = null;

            Assert.Throws<ArgumentNullException>(() => handler.Handle(request));
        }

        [Test]
        public void HandleThirdPartyResultQuery_QueryIsNotNull_CallsCreateProductResultService()
        {
            // Arrange
            cacheRefService.GetProductName(Arg.Any<int>()).Returns("ProductName");
            var query = new ThirdPartyResultQuery
            {
                ProductId = 6
            };

            // Act
            handler.Handle(query);

            // Assert
            serviceFactory.Received().CreateProductResultService(Arg.Is(query.ProductId));
        }

        [Test]
        public void HandleThirdPartyPopupResultQuery_QueryIsNull_ThrowArgumentNullException()
        {
            ThirdPartyDetailResultQuery request = null;

            Assert.Throws<ArgumentNullException>(() => handler.Handle(request));
        }

        [Test]
        public void HandleThirdPartyPopupResultQuery_QueryIsNotNull_CallsCreateProductResultService()
        {
            // Arrange
            cacheRefService.GetProductName(Arg.Any<int>()).Returns("ProductName");
            var query = new ThirdPartyDetailResultQuery
            {
                ProductId = 30
            };

            // Act
            handler.Handle(query);

            // Assert
            serviceFactory.Received().CreateProductResultService(Arg.Is(query.ProductId));
        }
    }
}
