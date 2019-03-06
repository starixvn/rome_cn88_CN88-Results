namespace Alpha.Results.Site.Tests.Controllers
{
    using System;
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Controllers;
    using Site.Helpers;
    using Site.Models.Requests;
    using Site.Models.Views;

    [TestFixture]
    public class NormalControllerTests
    {
        private IMediator mediator;
        private NormalController controller;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            controller = new NormalController(mediator);
        }

        [Test]
        public void Index_Always_CallsMediatorSendNormalResultQuery()
        {
            // Act
            controller.Index();

            // Assert
            mediator.Received().Send(Arg.Any<NormalResultQuery>());
        }

        [Test]
        public void ResultDetails_Always_CallsMediatorSendNormalResultDetailsQuery()
        {
            // Arrange
            var request = new NormalResultDetailsQuery();

            // Act
            controller.ResultDetails(request);

            // Assert
            mediator.Received().Send<NormalDetailViewModel>(Arg.Is(request));
        }

        [Test]
        public void ExcelResults_RequestIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => controller.ExcelResults(null)));
        }

        [Test]
        public void ExcelResults_RequestIsNotNull_CallMediatorSendExcelNormalResultQuery()
        {
            // Arrange
            var request = new ExcelNormalResultQuery();
            UrlHelper.ResultTemplatePath = "TemplatePath";

            var viewModel = new NormalResultExcelViewModel
            {
                ExcelResults = new byte[0]
            };
            mediator.Send<NormalResultExcelViewModel>(Arg.Any<ExcelNormalResultQuery>()).Returns(viewModel);

            // Act
            controller.ExcelResults(request);

            // Assert
            mediator.Received().Send<NormalResultExcelViewModel>(Arg.Is(request));
        }
    }
}
