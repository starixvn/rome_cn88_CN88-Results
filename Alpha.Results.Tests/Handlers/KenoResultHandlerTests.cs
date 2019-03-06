namespace Alpha.Results.Site.Tests.Handlers
{
    using System;
    using NUnit.Framework;
    using Site.Handlers;
    using Site.Models.Requests;

    [TestFixture]
    public class KenoResultHandlerTests
    {
        private KenoResultHandler _kenoResultHandler;

        [SetUp]
        public void SetUp()
        {
            _kenoResultHandler = new KenoResultHandler();
        }

        [Test]
        public void HandleKenoResultQuery_QueryIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => _kenoResultHandler.Handle(null)));
        }

        [Test]
        public void HandleKenoResultQuery_WhenCalled_ReturnResultUrl()
        {
            var query = new KenoResultQuery
            {
                CustId = 1,
                LanguageKey = "EN",
                RootUrl = "RootUrl/"
            };

            var actualViewModel = _kenoResultHandler.Handle(query);

            var expectedResultUrl = $"RootUrl/game/keno/kenoresult/resultdetail?date={DateTime.Today.ToString("yyyyMMdd")}&language=EN&productId=8&customerid=1";
            Assert.AreEqual(expectedResultUrl, actualViewModel.ResultUrl);
        }

        [Test]
        public void Handle_WhenCalled_ReturnExtendTokenUrl()
        {
            var query = new KenoResultQuery
            {
                CustId = 1,
                LanguageKey = "EN",
                RootUrl = "RootUrl/"
            };

            var actualViewModel = _kenoResultHandler.Handle(query);

            var expectedExtendTokenUrl = "RootUrl/game/keno/kenoresult/extendtoken?productId=8&userid=1";
            Assert.AreEqual(expectedExtendTokenUrl, actualViewModel.ExtendTokenUrl);
        }
    }
}
