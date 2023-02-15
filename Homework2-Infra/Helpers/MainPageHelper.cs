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
        private readonly IWebDriver _driver;

        #region Simple actions and locators
        public IReadOnlyCollection<IWebElement> GetAllProductCards() => _driver.FindElements(By.CssSelector("li.product"));
        public IWebElement GetInfoBlock(string id) => _driver.FindElement(By.CssSelector($"div#{id}"));
        public IReadOnlyCollection<IWebElement> GetAllProductCardsInsideInfoBlock(IWebElement context) => context.FindElements(By.CssSelector("li.product"));
        public IReadOnlyCollection<IWebElement> GetAllStickersInElement(IWebElement contextElement) => contextElement.FindElements(By.CssSelector("div.sticker"));
        public void ClickRegisterNewUser() => _driver.FindElement(By.CssSelector("form[name=login_form] a")).Click();
        public static By LogoutBy => By.CssSelector("div[id=box-account] li:nth-child(4) a");
        public void ClickLogout() => _driver.FindElement(LogoutBy).Click();
        #endregion

        public MainPageHelper(IWebDriver driver)
        {
            _driver = driver;
        }

        public void Login(string email, string password)
        {
            _driver.FindElement(By.CssSelector("input[name=email]")).SendKeys(email);
            _driver.FindElement(By.CssSelector("input[name=password]")).SendKeys(password);
            _driver.FindElement(By.CssSelector("button[name=login]")).Click();
        }
    }
}
