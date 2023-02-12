using OpenQA.Selenium;
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
            if(context.IsElementVisible(By.CssSelector("[class=price]")))
            {
                return (context.FindElement(By.CssSelector("[class=price]")), null);
            }
            else
            {
                return (context.FindElement(By.CssSelector("[class=regular-price]")),
                    context.FindElement(By.CssSelector("[class=campaign-price]")));
            }
        }
        #endregion

        public ProductPageHelper(IWebDriver driver)
        {
            _driver = driver;
        }
    }
}
