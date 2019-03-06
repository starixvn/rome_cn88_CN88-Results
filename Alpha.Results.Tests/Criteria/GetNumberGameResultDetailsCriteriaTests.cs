namespace Alpha.Results.Site.Tests.Criteria
{
    using NUnit.Framework;
    using Site.Criteria;

    [TestFixture]
    public class GetNumberGameResultDetailsCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new GetNumberGameResultDetailsCriteria();
            var expectedSettingKey = "Age_BetList_SelMatchResultNumberGame";

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
            var criteria = new GetNumberGameResultDetailsCriteria
            {
                MatchId = matchId
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
            var criteria = new GetNumberGameResultDetailsCriteria
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
