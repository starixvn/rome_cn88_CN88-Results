namespace Alpha.Results.Site.Tests.Models
{
    using NUnit.Framework;
    using Site.Models;

    [TestFixture]
    public class ActorTests
    {
        [Test]
        public void Constructor_Always_AssignCorrectProperties()
        {
            // Arrange
            var actor = new Actor(8, 4);

            // Act & Assert

            Assert.True(actor.ActorId == 8 && actor.ActorRoleId == 4);
        }

        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        public void IsValid_ByActorId_ReturnCorrectValue(int actorId, bool expectedValue)
        {
            // Arrange
            var actor = new Actor(actorId, 4);

            // Act
            var actualValue = actor.IsValid();

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCase(0, false)]
        [TestCase(1, true)]
        public void IsValid_ByActorRoleId_ReturnCorrectValue(byte actorRoleId, bool expectedValue)
        {
            // Arrange
            var actor = new Actor(1, actorRoleId);

            // Act
            var actualValue = actor.IsValid();

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
