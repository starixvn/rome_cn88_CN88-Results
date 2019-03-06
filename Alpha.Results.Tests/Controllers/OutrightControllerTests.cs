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
    public class OutrightControllerTests
    {
        private IMediator mediator;
        private OutrightController controller;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            controller = new OutrightController(mediator);
        }

        [Test]
        public void Index_Always_CallsMediatorSendOutrightResultQuery()
        {
            // Act
            controller.Index();

            // Assert
            mediator.Received().Send(Arg.Any<OutrightResultQuery>());
        }

        [Test]
        public void ExcelResults_RequestIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => controller.ExcelResults(null)));
        }

        [Test]
        public void ExcelResults_RequestIsNotNull_CallMediatorSendExcelOutrightResultQuery()
        {
            // Arrange
            var request = new ExcelOutrightResultQuery();
            UrlHelper.ResultTemplatePath = "TemplatePath";

            var viewModel = new OutrightResultExcelViewModel
            {
                ExcelResults = new byte[0]
            };
            mediator.Send<OutrightResultExcelViewModel>(Arg.Any<ExcelOutrightResultQuery>()).Returns(viewModel);

            // Act
            controller.ExcelResults(request);

            // Assert
            mediator.Received().Send<OutrightResultExcelViewModel>(Arg.Is(request));
        }
    }
}
