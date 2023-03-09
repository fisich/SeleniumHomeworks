using MultiLevelArchitecture.PageObjects;
using NUnit.Framework;
using System.Linq;

namespace MultiLevelArchitecture.Tests
{
    public class ShoppingTests : TestBase.TestBase
    {
        [Test, Description("Task 13. Add 3 products to cart, then delete them one by one")]
        public void AddAndRemoveProductsInCartTest()
        {
            GoToUrl(MainPage.BasePageUrl);
            while (Cart.GetProductsQuantityInCart() < 3)
            {
                MainPageHelper.WaitPageLoading();
                MainPageHelper.GetAllProductCards().First().Click();
                ProductPageHelper.WaitPageLoading();
                ProductPageHelper.SelectFirstSizeIfOptionExists();
                ProductPageHelper.ClickAddToCart();
                GoBack();
            }
            Cart.ClickCheckout();
            while (CheckoutPageHelper.GetOrderSummaryRows().Count != 0)
            {
                CheckoutPageHelper.RemoveProductFromCart();
            }
            Assert.That(CheckoutPageHelper.GetCheckoutMessage(), Is.EqualTo("There are no items in your cart."));
        }
    }
}
