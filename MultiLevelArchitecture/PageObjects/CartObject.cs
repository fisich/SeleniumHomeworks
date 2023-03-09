using MultiLevelArchitecture.Helpers;
using OpenQA.Selenium;
using System;

namespace MultiLevelArchitecture.PageObjects
{
    public class CartObject
    {
        private readonly IWebDriver _driver;

        #region Simple actions and locators
        public IWebElement CartProductQuantityElement => _driver.FindElement(By.CssSelector("div#cart span.quantity"));
        public int GetProductsQuantityInCart() => Int32.Parse(CartProductQuantityElement.Text);
        public void ClickCheckout() => _driver.FindElement(By.CssSelector("div#cart a.link")).Click();
        #endregion

        public CartObject(IWebDriver driver)
        {
            _driver = driver;
        }

        public void WaitProductAddToCart(string cartBeforeAction)
        {
            if (!CartProductQuantityElement.WaitElementUpdateCurrentText(cartBeforeAction))
                throw new ApplicationException("Cart info was not updated after add product");
        }
    }
}
