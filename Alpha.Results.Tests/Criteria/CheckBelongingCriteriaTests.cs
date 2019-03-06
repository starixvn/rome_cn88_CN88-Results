namespace Alpha.Results.Site.Tests.Criteria
{
    using NUnit.Framework;
    using Site.Criteria;

    [TestFixture]
    public class CheckBelongingCriteriaTests
    {
        private const int ActorIdTest = 8;
        private const int CustIdTest = 1176092;
        private const int SubAccountIdTest = 0;
        private const int TransIdTest = 0;

        [Test]
        public void GetSettingKey_Always_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new CheckBelongingCriteria(ActorIdTest, CustIdTest, SubAccountIdTest, TransIdTest);
            var expectedSettingKey = "Age_CustomerSelCheckBelonging";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_ValidInputParams_ReturnsTrue()
        {
            // Arrange
            var criteria = new CheckBelongingCriteria(ActorIdTest, CustIdTest, SubAccountIdTest, TransIdTest);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.IsTrue(actual);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void IsValid_InvalidActorId_ReturnsFalse(int actorId)
        {
            // Arrange
            var criteria = new CheckBelongingCriteria(actorId, CustIdTest, SubAccountIdTest, TransIdTest);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.IsFalse(actual);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void IsValid_InvalidCustId_ReturnsFalse(int custId)
        {
            // Arrange
            var criteria = new CheckBelongingCriteria(ActorIdTest, custId, SubAccountIdTest, TransIdTest);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.IsFalse(actual);
        }

        [TestCase(int.MinValue)]
        [TestCase(-1)]
        public void IsValid_InvalidSubAccountId_ReturnsFalse(int subAccountId)
        {
            // Arrange
            var criteria = new CheckBelongingCriteria(ActorIdTest, CustIdTest, subAccountId, TransIdTest);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.IsFalse(actual);
        }
    }
}
