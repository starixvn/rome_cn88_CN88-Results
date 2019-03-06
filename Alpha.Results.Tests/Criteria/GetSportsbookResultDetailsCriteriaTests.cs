namespace Alpha.Results.Site.Tests.Criteria
{
    using NUnit.Framework;
    using Site.Criteria;

    [TestFixture]
    public class GetSportsbookResultDetailsCriteriaTests
    {
        [Test]
        public void GetSettingKey_ViewResultFromBetList_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new GetSportsbookResultDetailsCriteria
            {
                IsViewResultFromBetList = true
            };

            var expectedSettingKey = "Age_BetList_SelMatchResult";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void GetSettingKey_ViewResultNotFromBetList_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new GetSportsbookResultDetailsCriteria
            {
                IsViewResultFromBetList = false
            };

            var expectedSettingKey = "Age_Result_MatchSelResultDetail";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void IsValid_InvalidMatchId_ReturnFalse(int matchId)
        {
            // Arrange
            var criteria = new GetSportsbookResultDetailsCriteria
            {
                MatchId = matchId
            };

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void IsValid_ValidCriteria_ReturnFalse()
        {
            // Arrange
            var criteria = new GetSportsbookResultDetailsCriteria
            {
                MatchId = 1
            };

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.IsTrue(isValid);
        }
    }
}
