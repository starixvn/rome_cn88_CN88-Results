namespace Alpha.Results.Site.Tests.Criteria
{
    using System;
    using NUnit.Framework;
    using Site.Criteria;

    [TestFixture]
    public class GetOutRightResultCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new GetOutRightResultCriteria();
            var expectedSettingKey = "Age_Result_Outright";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_InvalidFromDate_ReturnFalse()
        {
            // Arrange
            var criteria = new GetOutRightResultCriteria
            {
                FromDate = DateTime.MinValue,
                ToDate = DateTime.Now
            };

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void IsValid_InvalidToDate_ReturnFalse()
        {
            // Arrange
            var criteria = new GetOutRightResultCriteria
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.MinValue
            };

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void IsValid_InvalidDateRange_ReturnFalse()
        {
            // Arrange
            var criteria = new GetOutRightResultCriteria
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now.AddDays(-1)
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
            var criteria = new GetOutRightResultCriteria
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now
            };

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.IsTrue(isValid);
        }
    }
}
