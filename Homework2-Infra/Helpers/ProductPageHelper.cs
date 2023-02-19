using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework2_Infra.Helpers
{
    public class ProductPageHelper
    {
        private IWebDriver _driver;

        #region Simple actions and locators
        public string GetName() => _driver.FindElement(By.CssSelector("h1.title")).Text;
        public (IWebElement regularPrice, IWebElement campaignPrice) GetPriceElements()
        {
            return GetPriceElementsInContext(_driver.FindElement(By.CssSelector("div.information")));
        }
        public static (IWebElement regularPrice, IWebElement campaignPrice) GetPriceElementsInContext(IWebElement context)
        {
            if (context.IsElementVisible(By.CssSelector("[class=price]")))
            {
                return (context.FindElement(By.CssSelector("[class=price]")), null);
            }
            else
            {
                return (context.FindElement(By.CssSelector("[class=regular-price]")),
                    context.FindElement(By.CssSelector("[class=campaign-price]")));
            }
        }
        public void ClickAddToCart() => _driver.FindElement(By.CssSelector("button[name=add_cart_product]")).Click();
        public void SelectFirstSizeIfOptionExists()
        {
            var locator = By.XPath("//select[@name='options[Size]']");
            if (_driver.IsElementEnabled(locator))
                new SelectElement(_driver.FindElement(locator)).SelectByIndex(1);
        }
        #endregion

        public ProductPageHelper(IWebDriver driver)
        {
            _driver = driver;
        }
    }
}
