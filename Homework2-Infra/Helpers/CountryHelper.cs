using OpenQA.Selenium;
using System.Collections.Generic;

namespace Homework2_Infra.Helpers
{
    public class CountryHelper
    {
        private readonly IWebDriver _driver;

        #region Simple actions and locators
        public void ClickAddCountry() => _driver.FindElement(By.CssSelector("td[id=content] a.button")).Click();
        public IReadOnlyCollection<IWebElement> GetExternalLinks() => _driver.FindElements(By.CssSelector("i.fa.fa-external-link"));
        #endregion

        public CountryHelper(IWebDriver driver)
        {
            _driver = driver;
        }
    }
}
