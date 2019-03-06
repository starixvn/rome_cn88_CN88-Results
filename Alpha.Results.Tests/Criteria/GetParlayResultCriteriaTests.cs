namespace Alpha.Results.Site.Tests.Criteria
{
    using NUnit.Framework;
    using Site.Criteria;

    [TestFixture]
    public class GetParlayResultCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new GetParlayResultCriteria();
            var expectedSettingKey = "Age_BetList_SelMatchResultParlay";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void IsValid_InvalidRefNo_ReturnFalse(long refNo)
        {
            // Arrange
            var criteria = new GetParlayResultCriteria
            {
                RefNo = refNo
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
            var criteria = new GetParlayResultCriteria
            {
                RefNo = 1
            };

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.IsTrue(isValid);
        }
    }
}
