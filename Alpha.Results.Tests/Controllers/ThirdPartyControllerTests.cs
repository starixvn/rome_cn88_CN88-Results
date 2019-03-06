namespace Alpha.Results.Site.Tests.Controllers
{
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Controllers;
    using Site.Models.Requests;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class ThirdPartyControllerTests
    {
        private IMediator mediator;
        private IUserSession userSession;
        private ThirdPartyController controller;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();

            userSession = Substitute.For<IUserSession>();
            userSession.Language.Returns("en-Us");

            controller = new ThirdPartyController(mediator, userSession);
        }

        [Test]
        public void Index_Always_CallsMediatorSendThirdPartyQuery()
        {
            // Arrange
            var query = new ThirdPartyResultQuery();

            // Act
            controller.Index(query);

            // Assert
            mediator.Received().Send(Arg.Is<ThirdPartyResultQuery>(msg => msg.Language == userSession.Language));
        }

        [Test]
        public void Detail_Always_CallsMediatorSendThirdPartyDetailResultQuery()
        {
            // Arrange
            var query = new ThirdPartyDetailResultQuery();

            // Act
            controller.Detail(query);

            // Assert
            mediator.Received().Send(Arg.Is<ThirdPartyDetailResultQuery>(msg => msg.Language == userSession.Language));
        }
    }
}
