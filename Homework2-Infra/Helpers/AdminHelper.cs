using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework2_Infra
{
    public class AdminHelper
    {
        public static string BasePageUrl { get; private set; } = "http://localhost/litecart/admin/";
        private IWebDriver _driver;

        #region Simple actions and locators
        public IWebElement GetLoginField() => _driver.FindElement(By.XPath("//input[@name='username']"));
        public void EnterLogin(string username) => GetLoginField().SendKeys(username);
        public IWebElement GetPasswordField() => _driver.FindElement(By.XPath("//input[@name='password']"));
        public void EnterPassword(string password) => GetPasswordField().SendKeys(password);
        public void PressLoginButton() => _driver.FindElement(By.XPath("//button[@name='login']")).Click();
        public IReadOnlyCollection<IWebElement> GetAllTabs() => _driver.FindElements(By.CssSelector("ul#box-apps-menu > li"));
        public IReadOnlyCollection<IWebElement> GetAllSubTabs() => _driver.FindElements(By.CssSelector("ul#box-apps-menu > li li"));
        public By ByTabHeader() => By.CssSelector("td#content > h1");
        #endregion

        public AdminHelper(IWebDriver driver)
        {
            _driver = driver;
        }

        public void LoginAsAdmin(string username, string password)
        {
            EnterLogin(username);
            EnterPassword(password);
            PressLoginButton();
        }
    }
}
