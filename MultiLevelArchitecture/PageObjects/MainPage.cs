using MultiLevelArchitecture.Helpers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace MultiLevelArchitecture.PageObjects
{
    public class MainPage
    {
        public static string BasePageUrl { get; private set; } = "http://localhost/litecart/";
        private readonly IWebDriver _driver;

        #region Simple actions and locators
        public IReadOnlyCollection<IWebElement> GetAllProductCards() => _driver.FindElements(By.CssSelector("li.product"));
        #endregion

        public MainPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void WaitPageLoading()
        {
            if (_driver.WaitElementVisible(By.CssSelector($"div#box-most-popular")) == null)
                throw new ApplicationException("Most popular title not found on main page");
        }
    }
}
