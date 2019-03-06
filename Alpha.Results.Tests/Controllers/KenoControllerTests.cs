namespace Alpha.Results.Site.Tests.Controllers
{
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Controllers;
    using Site.Helpers;
    using Site.Models.Requests;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class KenoControllerTests
    {
        private IMediator mediator;
        private IUserSession userSession;
        private KenoController controller;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();

            userSession = Substitute.For<IUserSession>();
            userSession.LangKey.Returns("EN");
            userSession.CustId.Returns(1);
            userSession.RoleId.Returns((byte)2);

            UrlHelper.CurrentAppPath = "path";

            controller = new KenoController(mediator, userSession);
        }

        [Test]
        public void Index_Always_CallMediatorSendKenoResultQuery()
        {
            // Act
            controller.Index();

            // Assert
            mediator.Received().Send(Arg.Is<KenoResultQuery>(
                query => query.LanguageKey == userSession.LangKey
                    && query.CustId == userSession.CustId
                    && query.CustLevelId == userSession.RoleId
                    && query.RootUrl == "path"));
        }
    }
}
