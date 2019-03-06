namespace Alpha.Results.Site.Tests.Services
{
    using Fanex.RemotingRef;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Services.Impl;

    [TestFixture]
    public class CacheRefServiceTests
    {
        private IRemotingRefService remotingRefService;
        private CacheRefService cacheRefService;

        [SetUp]
        public void Setup()
        {
            remotingRefService = Substitute.For<IRemotingRefService>();
            cacheRefService = new CacheRefService(remotingRefService);
        }

        [Test]
        public void GetProduct_Awlays_SetProduct()
        {
            //// Arrange

            //// Act
            var act = cacheRefService.GetProductName(16);

            //// Assert
            remotingRefService.Received().GetResourceDataByRefIdAndResourceId(Arg.Is("Product"), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }
    }
}
