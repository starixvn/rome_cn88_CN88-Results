namespace Alpha.Results.Site.Tests.Configurations
{
    using System.Collections.Specialized;
    using NUnit.Framework;
    using Site.Configurations;

    [TestFixture]
    public class AppSettingsTests
    {
        [Test]
        public void NG2SiteAppPath_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["NG2SiteVirtualPath"] = "Path"
            };
            var expectedSetting = "Path";

            // Act
            var actualSetting = AppSettings.NG2SiteAppPath;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

        [Test]
        public void NG2SiteAppPath_NotHaveConfig_ReturnsNull()
        {
            // Arrange
            AppSettings.AppSettingValues = new NameValueCollection();

            // Act
            var actualSetting = AppSettings.NG2SiteAppPath;

            // Assert
            Assert.Null(actualSetting);
        }

        [Test]
        public void LocalTest_ConfigValueIsOne_ReturnsTrue()
        {
            // Arrange
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["LocalTest"] = "1"
            };

            // Act
            var actualValue = AppSettings.LocalTest;

            // Assert
            Assert.True(actualValue);
        }

        [Test]
        public void LocalTest_ConfigValueIsZero_ReturnsFalse()
        {
            // Arrange
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["LocalTest"] = "0"
            };

            // Act
            var actualValue = AppSettings.LocalTest;

            // Assert
            Assert.False(actualValue);
        }

        [Test]
        public void LocalTest_NotHaveConfig_ReturnsFalse()
        {
            // Arrange
            AppSettings.AppSettingValues = new NameValueCollection();

            // Act
            var actualValue = AppSettings.LocalTest;

            // Assert
            Assert.False(actualValue);
        }

        [Test]
        public void Disable151_31_ConfigValueIsOne_ReturnsTrue()
        {
            // Arrange
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["Disable151_31"] = "1"
            };

            // Act
            var actualValue = AppSettings.Disable151_31;

            // Assert
            Assert.True(actualValue);
        }

        [Test]
        public void Disable151_31_ConfigValueIsZero_ReturnsFalse()
        {
            // Arrange
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["Disable151_31"] = "0"
            };

            // Act
            var actualValue = AppSettings.Disable151_31;

            // Assert
            Assert.False(actualValue);
        }

        [Test]
        public void Disable151_31_NotHaveConfig_ReturnsFalse()
        {
            // Arrange
            AppSettings.AppSettingValues = new NameValueCollection();

            // Act
            var actualValue = AppSettings.Disable151_31;

            // Assert
            Assert.False(actualValue);
        }

        [Test]
        public void RemoteRefURL_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["RemoteRefURL"] = "Path"
            };
            var expectedSetting = "Path";

            // Act
            var actualSetting = AppSettings.RemoteRefURL;

            // Assert
            Assert.AreEqual(expectedSetting, actualSetting);
        }

    }
}
