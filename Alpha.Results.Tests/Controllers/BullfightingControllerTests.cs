namespace Alpha.Results.Site.Tests.Controllers
{
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Constants;
    using Site.Controllers;
    using Site.Models.Requests;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class BullfightingControllerTests
    {
        private IMediator mediator;
        private IUserSession userSession;
        private BullfightingController controller;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();

            userSession = Substitute.For<IUserSession>();
            userSession.Language.Returns("en-Us");

            controller = new BullfightingController(mediator, userSession);
        }

        [Test]
        public void Index_Always_CallsMediatorSendThirdPartyPopupQuery()
        {
            // Arrange

            // Act
            controller.Index();

            // Assert
            mediator.Received()
                .Send(Arg.Is<ThirdPartyResultQuery>(
                    msg => msg.Language == userSession.Language
                    && msg.CustId == userSession.CustId
                    && msg.RoleId == userSession.RoleId
                    && msg.ProductId == ProductIdConstants.Bullfighting));
        }
    }
}
