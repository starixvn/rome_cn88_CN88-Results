namespace Alpha.Results.Site.Tests.Models.Views
{
    using System;
    using NUnit.Framework;
    using Site.Constants;
    using Site.Models.Views;

    [TestFixture]
    public class TabViewModelTests
    {
        [Test]
        public void SelectedTabName_Always_ReturnCorrectValue()
        {
            // Arrange
            var tabName = "TabName";
            Func<int, string> getProductName = (productId) =>
            {
                switch (productId)
                {
                    case ProductIdConstants.NG2:
                        return "NG2";

                    case ProductIdConstants.Lottery:
                        return "Lottery";

                    case ProductIdConstants.Bullfighting:
                        return "Bullfighting";
                }

                return string.Empty;
            };
            var viewModel = new TabViewModel(tabName, false, false, false, false, getProductName);
            var expectedValue = "tabname";

            // Act
            var actualValue = viewModel.SelectedTabName;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void IsEnableBullfighting_Always_CorrectValueInConstructor()
        {
            // Arrange
            var isEnableBullfighting = true;
            Func<int, string> getProductName = (productId) =>
            {
                switch (productId)
                {
                    case ProductIdConstants.NG2:
                        return "NG2";

                    case ProductIdConstants.Lottery:
                        return "Lottery";

                    case ProductIdConstants.Bullfighting:
                        return "Bullfighting";
                }

                return string.Empty;
            };
            var viewModel = new TabViewModel("test", false, false, false, isEnableBullfighting, getProductName);

            // Act
            var actualValue = viewModel.IsEnableBullfighting;

            // Assert
            Assert.AreEqual(isEnableBullfighting, actualValue);
        }
    }
}
