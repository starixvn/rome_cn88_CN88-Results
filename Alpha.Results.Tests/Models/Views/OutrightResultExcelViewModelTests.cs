namespace Alpha.Results.Site.Tests.Models.Views
{
    using NUnit.Framework;
    using Site.Models.Views;

    [TestFixture]
    public class OutrightResultExcelViewModelTests
    {
        [Test]
        public void ExcelName_Always_ReturnCorrectValue()
        {
            // Arrange
            var expectedValue = "OutrightResults.xls";
            var viewModel = new OutrightResultExcelViewModel();

            // Act
            var actualValue = viewModel.ExcelName;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
