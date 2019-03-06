using NUnit.Framework;
namespace Alpha.Results.Site.Tests.Services
{
    using NSubstitute;
    using NUnit.Framework;
    using Site.Constants;
    using Site.Services.Impl;
    using Site.Services.Impl.Products;

    [TestFixture]
    public class ResultServiceFactoryTests
    {
        private ResultServiceFactory _resultServiceFactory;

        [SetUp]
        public void SetUp()
        {
            _resultServiceFactory = new ResultServiceFactory();
        }

        [TestCase(9)]
        [TestCase(29)]
        public void CreateProductResultService_BetTypeIsMixParlayOrSystemParlay_ReturnParlayResultService(int betTypeId)
        {
            //// Act
            var resultService = _resultServiceFactory.CreateProductResultService(0, betTypeId);

            //// Assert
            Assert.IsTrue(resultService is ParlayResultService);
        }

        [Test]
        public void CreateProductResultService_SportIsSoccerAndBetTypeIsInRangeOfMetricGaming_ReturnMetricGamingResultService(
            [Range(801, 899, 1)] int betTypeId)
        {
            //// Act
            var sportId = 1;
            var resultService = _resultServiceFactory.CreateProductResultService(sportId, betTypeId);

            //// Assert
            Assert.IsTrue(resultService is MetricGamingResultService);
        }

        [TestCase(SportTypeIdConstants.Soccer)]
        [TestCase(SportTypeIdConstants.BasketBall)]
        [TestCase(SportTypeIdConstants.Netball)]
        [TestCase(SportTypeIdConstants.FootBall)]
        [TestCase(SportTypeIdConstants.Rugby)]
        [TestCase(SportTypeIdConstants.IceHockey)]
        [TestCase(SportTypeIdConstants.FieldHockey)]
        [TestCase(SportTypeIdConstants.Tennis)]
        [TestCase(SportTypeIdConstants.VolleyBall)]
        [TestCase(SportTypeIdConstants.Badminton)]
        [TestCase(SportTypeIdConstants.Squash)]
        [TestCase(SportTypeIdConstants.BaseBall)]
        [TestCase(SportTypeIdConstants.Golf)]
        [TestCase(SportTypeIdConstants.Motorsports)]
        [TestCase(SportTypeIdConstants.Billard)]
        [TestCase(SportTypeIdConstants.Cycling)]
        [TestCase(SportTypeIdConstants.Fencing)]
        [TestCase(SportTypeIdConstants.Judo)]
        [TestCase(SportTypeIdConstants.ModernPentathlon)]
        [TestCase(SportTypeIdConstants.Rowing)]
        [TestCase(SportTypeIdConstants.Sailing)]
        [TestCase(SportTypeIdConstants.Shooting)]
        [TestCase(SportTypeIdConstants.Taekwondo)]
        [TestCase(SportTypeIdConstants.Triathlon)]
        [TestCase(SportTypeIdConstants.Wrestling)]
        [TestCase(SportTypeIdConstants.Winter)]
        [TestCase(SportTypeIdConstants.ESports)]
        [TestCase(SportTypeIdConstants.VirtualSports)]
        [TestCase(SportTypeIdConstants.VirtualTennis)]
        public void CreateProductResultService_SportIsBelongingToSportsbook_ReturnSportsbookResultService(
            int sportId)
        {
            //// Act
            var resultService = _resultServiceFactory.CreateProductResultService(sportId, 0);

            //// Assert
            Assert.IsTrue(resultService is SportsbookResultService);
        }

        [Test]
        public void CreateProductResultService_SportIsNumbergame_ReturnNumbergameResultService()
        {
            //// Act
            var resultService = _resultServiceFactory.CreateProductResultService(SportTypeIdConstants.NumberGame, Arg.Any<int>());

            //// Assert
            Assert.IsTrue(resultService is NumberGameResultService);
        }

        [Test]
        public void CreateProductResultService_SportIsLucky3_ReturnLucky3ResultService()
        {
            //// Act
            var resultService = _resultServiceFactory.CreateProductResultService(SportTypeIdConstants.Lucky3, Arg.Any<int>());

            //// Assert
            Assert.IsTrue(resultService is Lucky3ResultService);
        }        

        [Test]
        public void CreateProductResultService_SportIsAllBet_ReturnAllBetResultService()
        {
            //// Act
            var resultService = _resultServiceFactory.CreateProductResultService(SportTypeIdConstants.AllBet, Arg.Any<int>());

            //// Assert
            Assert.IsTrue(resultService is AllBetCasinoResultService);
        }

        [TestCase(SportTypeIdConstants.VirtualHorsesRacing)]
        [TestCase(SportTypeIdConstants.VirtualGreyhounds)]
        [TestCase(SportTypeIdConstants.VirtualMotors)]
        [TestCase(SportTypeIdConstants.VirtualCycling)]
        [TestCase(SportTypeIdConstants.VirtualSpeedway)]
        public void CreateProductResultService_SportIsBelongToVirtualRacing_ReturnVirtualRacingResultService(
            int sportId)
        {
            //// Act
            var resultService = _resultServiceFactory.CreateProductResultService(sportId, 0);

            //// Assert
            Assert.IsTrue(resultService is VirtualRacingResultService);
        }

        [Test]
        public void CreateProductResultService_SportIsGoldDeluxe_ReturnGoldDeluxeResultService()
        {
            //// Act
            var resultService = _resultServiceFactory.CreateProductResultService(SportTypeIdConstants.GoldDeluxe, Arg.Any<int>());

            //// Assert
            Assert.IsTrue(resultService is GoldDeluxeResultService);
        }

        [TestCase(SportTypeIdConstants.Athletics)]
        [TestCase(SportTypeIdConstants.Equestrian)]
        public void CreateProductResultService_OtherSports_ReturnSportsbookResultService(
            int sportId)
        {
            //// Act
            var resultService = _resultServiceFactory.CreateProductResultService(sportId, 0);

            //// Assert
            Assert.IsTrue(resultService is SportsbookResultService);
        }

        [Test]
        public void CreateProductResultService_SportTypeIsHappy5_ReturnHappy5Service()
        {
            //// Arrange
            var sportTypeIsHappy5 = 164;

            //// Act
            var act = _resultServiceFactory.CreateProductResultService(sportTypeIsHappy5, 0);

            //// Assert
            Assert.IsTrue(act is Happy5ResultService);
        }
    }
}
