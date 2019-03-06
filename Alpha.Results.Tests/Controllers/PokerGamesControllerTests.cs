namespace Alpha.Results.Site.Tests.Controllers
{
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Controllers;
    using Site.Models.Requests;
    using Site.Services;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class PokerGamesControllerTests
    {
        private IMediator mediator;
        private IUserSession userSession;
        private IAuthorizationService authorizationService;
        private PokerGamesController controller;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            authorizationService = Substitute.For<IAuthorizationService>();

            userSession = Substitute.For<IUserSession>();
            userSession.Language.Returns("en-Us");

            controller = new PokerGamesController(mediator, userSession, authorizationService);
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
