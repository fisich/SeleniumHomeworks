using MultiLevelArchitecture.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiLevelArchitecture.PageObjects
{
    public class ProductPage
    {
        private readonly IWebDriver _driver;
        #region Simple actions and locators
        public void ClickAddToCart() {
            CartObject cart = new CartObject(_driver);
            var quantityTextBeforeAddToCart = cart.GetProductsQuantityInCart();
            _driver.FindElement(By.CssSelector("button[name=add_cart_product]")).Click();
            cart.WaitProductAddToCart(quantityTextBeforeAddToCart.ToString());
        }
        public By ProductSizeOptionBy => By.XPath("//select[@name='options[Size]']");
        #endregion

        public ProductPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void SelectFirstSizeIfOptionExists()
        {
            if (_driver.IsElementEnabled(ProductSizeOptionBy))
                new SelectElement(_driver.FindElement(ProductSizeOptionBy)).SelectByIndex(1);
        }

        public void WaitPageLoading()
        {
            if (_driver.WaitElementVisible(By.CssSelector("h1.title")) == null)
                throw new ApplicationException("Product page not loaded or h1 header not found");
        }
    }
}
