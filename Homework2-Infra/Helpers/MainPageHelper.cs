using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework2_Infra.Helpers
{
    public class MainPageHelper
    {
        public static string BasePageUrl { get; private set; } = "http://localhost/litecart/";
        private IWebDriver _driver;

        #region Simple actions and locators
        public IReadOnlyCollection<IWebElement> GetAllProductCards() => _driver.FindElements(By.CssSelector("li.product"));
        public IWebElement GetInfoBlock(string id) => _driver.FindElement(By.CssSelector($"div#{id}"));
        public IReadOnlyCollection<IWebElement> GetAllProductCardsInsideInfoBlock(IWebElement context) => context.FindElements(By.CssSelector("li.product"));
        public IReadOnlyCollection<IWebElement> GetAllStickersInElement(IWebElement contextElement) => contextElement.FindElements(By.CssSelector("div.sticker"));
        #endregion

        public MainPageHelper(IWebDriver driver)
        {
            _driver = driver;
        }
    }
}
