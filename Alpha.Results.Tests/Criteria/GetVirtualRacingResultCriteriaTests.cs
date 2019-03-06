namespace Alpha.Results.Site.Tests.Criteria
{
    using System;
    using System.Collections.Specialized;
    using NUnit.Framework;
    using Site.Configurations;
    using Site.Criteria;

    [TestFixture]
    public class GetVirtualRacingResultCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new GetVirtualRacingResultCriteria();
            var expectedSettingKey = "Age_Result_VSRaceSelByMeeting";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_InvalidFromDate_ReturnFalse()
        {
            // Arrange
            var criteria = new GetVirtualRacingResultCriteria
            {
                FromDate = DateTime.MinValue,
                SportType = 1
            };

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void IsValid_InvalidSportType_ReturnFalse(int sportType)
        {
            // Arrange
            var criteria = new GetVirtualRacingResultCriteria
            {
                FromDate = DateTime.Now,
                SportType = sportType
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
            var criteria = new GetVirtualRacingResultCriteria
            {
                FromDate = DateTime.Now,
                SportType = 1
            };

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void IsShowAll_LocalTestIsTrue_ReturnsTrue()
        {
            // Arrange
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["LocalTest"] = "1"
            };
            var criteria = new GetVirtualRacingResultCriteria();

            // Act
            var isShowAll = criteria.IsShowAll;

            // Assert
            Assert.True(isShowAll);
        }

        [Test]
        public void IsShowAll_LocalTestIsFalse_ReturnsTrue()
        {
            // Arrange
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["LocalTest"] = "0"
            };
            var criteria = new GetVirtualRacingResultCriteria();

            // Act
            var isShowAll = criteria.IsShowAll;

            // Assert
            Assert.False(isShowAll);
        }
    }
}
