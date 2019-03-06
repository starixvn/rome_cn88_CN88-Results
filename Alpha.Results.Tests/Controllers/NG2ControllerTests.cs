namespace Alpha.Results.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using NUnit.Framework;
    using Site.Controllers;

    [TestFixture]
    public class NG2ControllerTests
    {
        private NG2Controller controller;

        [SetUp]
        public void SetUp()
        {
            Site.Helpers.UrlHelper.NG2ResultPath = "NG2ResultPath";
            controller = new NG2Controller();
        }

        [Test]
        public void Index_Always_ReturnViewResult()
        {
            // Act
            var viewResult = controller.Index() as ViewResult;

            // Assert
            Assert.IsTrue(
                viewResult.ViewName.Equals("~/Views/NG2/Index.cshtml") &&
                viewResult.Model.Equals("NG2ResultPath/All"));
        }

        [Test]
        public void TicketDetail_Always_RedirectToCorrectAction()
        {
            // Arrange
            var gambleId = 1;
            var routeValues = new RouteValueDictionary(new { gambleId });

            // Act
            var redirectResult = controller.TicketDetail(gambleId) as RedirectToRouteResult;

            // Assert
            Assert.IsTrue(redirectResult.RouteValues["action"].Equals("TicketDetail")
                && redirectResult.RouteValues["controller"].Equals("..NG2ResultPath")
                && redirectResult.RouteValues["gambleId"].Equals(gambleId));
        }

        [Test]
        public void ParlayDetail_Always_RedirectToCorrectAction()
        {
            // Arrange
            string refno = "1";
            var routeValues = new RouteValueDictionary(new { refno });

            // Act
            var redirectResult = controller.ParlayDetail(refno) as RedirectToRouteResult;

            // Assert
            Assert.IsTrue(redirectResult.RouteValues["action"].Equals("ParlayDetail")
                && redirectResult.RouteValues["controller"].Equals("..NG2ResultPath")
                && redirectResult.RouteValues["refno"].Equals(refno));
        }
    }
}
