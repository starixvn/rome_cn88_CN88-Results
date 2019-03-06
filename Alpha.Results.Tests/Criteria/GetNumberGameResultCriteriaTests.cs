namespace Alpha.Results.Site.Tests.Criteria
{
    using System;
    using NUnit.Framework;
    using Site.Constants;
    using Site.Criteria;

    [TestFixture]
    public class GetNumberGameResultCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new GetNumberGameResultCriteria();
            var expectedSettingKey = "Age_Result_MatchSelNumberGameByLeague";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_InvalidFromDate_ReturnFalse()
        {
            // Arrange
            var criteria = new GetNumberGameResultCriteria
            {
                FromDate = DateTime.MinValue,
                LeagueId = 1,
                SportType = SportTypeIdConstants.NumberGame
            };

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void IsValid_InvalidLeagueId_ReturnFalse(int leagueId)
        {
            // Arrange
            var criteria = new GetNumberGameResultCriteria
            {
                FromDate = DateTime.Now,
                LeagueId = leagueId,
                SportType = SportTypeIdConstants.NumberGame
            };

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void IsValid_ValidCriteria_ReturnTrue()
        {
            // Arrange
            var criteria = new GetNumberGameResultCriteria
            {
                FromDate = DateTime.Now,
                LeagueId = 1,
                SportType = SportTypeIdConstants.NumberGame
            };

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.IsTrue(isValid);
        }
    }
}
