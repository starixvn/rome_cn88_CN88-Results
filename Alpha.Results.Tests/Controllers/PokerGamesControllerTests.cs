namespace Alpha.Results.Site.Tests.Controllers
{
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Controllers;
    using Site.Models.Requests;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class PokerGamesControllerTests
    {
        private IMediator mediator;
        private IUserSession userSession;
        private PokerGamesController controller;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();

            userSession = Substitute.For<IUserSession>();
            userSession.Language.Returns("en-Us");

            controller = new PokerGamesController(mediator, userSession);
        }

        [Test]
        public void Index_Always_CallsMediatorSendPokerGamesResultQuery()
        {
            // Arrange
            var query = new PokerGamesResultQuery();

            // Act
            controller.Index(query);

            // Assert
            mediator.Received().Send(Arg.Is<PokerGamesResultQuery>(msg => msg.RoleId == userSession.RoleId));
        }
    }
}
