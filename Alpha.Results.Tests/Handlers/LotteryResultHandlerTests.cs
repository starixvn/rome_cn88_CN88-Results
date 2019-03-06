namespace Alpha.Results.Site.Tests.Handlers
{
    using System;
    using NUnit.Framework;
    using Site.Handlers;
    using Site.Models.Requests;

    [TestFixture]
    public class LotteryResultHandlerTests
    {
        private LotteryResultHandler _lotteryResultHandler;

        [SetUp]
        public void SetUp()
        {
            _lotteryResultHandler = new LotteryResultHandler();
        }

        [Test]
        public void HandleLotteryResultQuery_WhenCalled_ReturnResultUrl()
        {
            var query = new LotteryResultQuery
            {
                CustId = 1,
                LanguageKey = "EN",
                RootPath = "RootUrl/"
            };

            var actualViewModel = _lotteryResultHandler.Handle(query);

            var expectedResultPath = $"RootUrl/game/keno/kenoresult/resultdetail?date={DateTime.Today.ToString("yyyyMMdd")}&language=EN&productId=28&customerid=1";
            Assert.AreEqual(expectedResultPath, actualViewModel.ResultPath);
        }

        [Test]
        public void Handle_WhenCalled_ReturnExtendTokenUrl()
        {
            var query = new LotteryResultQuery
            {
                CustId = 1,
                LanguageKey = "EN",
                RootPath = "RootUrl/"
            };

            var actualViewModel = _lotteryResultHandler.Handle(query);

            var expectedExtendTokenPath = "RootUrl/game/keno/kenoresult/extendtoken?productId=28&userid=1";
            Assert.AreEqual(expectedExtendTokenPath, actualViewModel.ExtendTokenPath);
        }
    }
}
