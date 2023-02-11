using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework2_Infra.Helpers
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

        #region CountriesTab actions and locators
        public IReadOnlyCollection<IWebElement> GetCountriesList() => _driver.FindElements(By.CssSelector("table.dataTable tr.row"));
        public (int Id, string Code, string Name, int Zones, string editPageUrl) GetCountryInfo(IWebElement countryRow)
        {
            var cols = countryRow.FindElements(By.CssSelector("td"));
            return (Int32.Parse(cols[2].Text), cols[3].Text, cols[4].Text, Int32.Parse(cols[5].Text), cols[6].FindElement(By.CssSelector("a")).GetAttribute("href"));
        }
        public IEnumerable<IWebElement> GetZonesList()
        {
            var dataRows = _driver.FindElements(By.CssSelector("table.dataTable tr:not([class=header])"));
            return dataRows.Take(dataRows.Count - 1);
        }
        public (int Id, string Code, string Name, IWebElement deleteButton) GetZoneInfo(IWebElement zoneRow)
        {
            var cols = zoneRow.FindElements(By.CssSelector("td"));
            return (Int32.Parse(cols[0].Text), cols[1].Text, cols[2].Text, cols[3]);
        }
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
