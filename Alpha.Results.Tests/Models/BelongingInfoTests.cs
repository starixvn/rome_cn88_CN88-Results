namespace Alpha.Results.Site.Tests.Models
{
    using NUnit.Framework;
    using Site.Models;

    [TestFixture]
    public class BelongingInfoTests
    {
        [Test]
        public void Constructor_Always_AssignCorrectProperties()
        {
            // Arrange
            var belongingInfo = new BelongingInfo(4, true);

            // Act & Assert

            Assert.True(belongingInfo.RoleId == 4 && belongingInfo.IsActive);
        }
    }
}
