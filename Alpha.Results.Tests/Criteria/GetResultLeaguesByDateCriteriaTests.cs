namespace Alpha.Results.Site.Tests.Criteria
{
    using System;
    using System.Collections.Specialized;
    using NUnit.Framework;
    using Site.Configurations;
    using Site.Criteria;

    [TestFixture]
    public class GetResultLeaguesByDateCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new GetResultLeaguesByDateCriteria(DateTime.Now);
            var expectedSettingKey = "Age_Result_LeagueSelByDate";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsShowAll_LocalTestIsTrue_ReturnsTrue()
        {
            // Arrange
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["LocalTest"] = "1"
            };
            var criteria = new GetResultLeaguesByDateCriteria(DateTime.Now);

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
            var criteria = new GetResultLeaguesByDateCriteria(DateTime.Now);

            // Act
            var isShowAll = criteria.IsShowAll;

            // Assert
            Assert.False(isShowAll);
        }
    }
}
