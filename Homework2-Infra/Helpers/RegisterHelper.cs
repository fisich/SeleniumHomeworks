using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Linq;

namespace Homework2_Infra.Helpers
{
    public class RegistrationHelper
    {
        private readonly IWebDriver _driver;

        #region Simple actions and locators
        public void EnterTaxId(string taxId) => _driver.FindElement(By.CssSelector("input[name=tax_id]")).SendKeys(taxId);
        public void EnterCompany(string company) => _driver.FindElement(By.CssSelector("input[name=tax_id]")).SendKeys(company);
        public void EnterFirstname(string firstname) => _driver.FindElement(By.CssSelector("input[name=firstname]")).SendKeys(firstname);
        public void EnterLastname(string lastname) => _driver.FindElement(By.CssSelector("input[name=lastname]")).SendKeys(lastname);
        public void EnterAddress1(string address1) => _driver.FindElement(By.CssSelector("input[name=address1]")).SendKeys(address1);
        public void EnterAddress2(string address2) => _driver.FindElement(By.CssSelector("input[name=address2]")).SendKeys(address2);
        public void EnterPostcode(string postcode) => _driver.FindElement(By.CssSelector("input[name=postcode]")).SendKeys(postcode);
        public void EnterCity(string city) => _driver.FindElement(By.CssSelector("input[name=city]")).SendKeys(city);

        public void SelectCountry(string country)
        {
            _driver.FindElement(By.CssSelector("span.select2")).Click();
            var dropdown = _driver.FindElement(By.CssSelector("span.select2-dropdown"));
            var elements = dropdown.FindElements(By.CssSelector("li.select2-results__option"));
            var selectElement = elements.First(e => e.Text.Contains(country));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", selectElement);
            selectElement.Click();
        }

        public void SelectState(string state)
        {
            var select = new SelectElement(_driver.FindElement(By.CssSelector("select[name=zone_code]")));
            select.SelectByText(state);
        }
        public void EnterEmail(string email) => _driver.FindElement(By.CssSelector("input[name=email]")).SendKeys(email);
        public void EnterPhone(string phone) => _driver.FindElement(By.CssSelector("input[name=phone]")).SendKeys(phone);
        public void EnterPassword(string password) => _driver.FindElement(By.CssSelector("input[name=password")).SendKeys(password);
        public void EnterConfirmPassword(string password) => _driver.FindElement(By.CssSelector("input[name=confirmed_password]")).SendKeys(password);
        public void ClickCreateAccount() => _driver.FindElement(By.CssSelector("button[name=create_account]")).Click();
        #endregion

        public RegistrationHelper(IWebDriver driver)
        {
            _driver = driver;
        }

        public void QuickFillInputForm(string firstname, string lastname, string address1, string postcode,
            string city, string county, string email, string phone, string password)
        {
            EnterFirstname(firstname);
            EnterLastname(lastname);
            EnterAddress1(address1);
            EnterPostcode(postcode);
            EnterCity(city);
            SelectCountry(county);
            var selectElement = _driver.WaitElementEnabled(By.CssSelector("select[name=zone_code]")); // dynamic list
            var select = new SelectElement(selectElement);
            select.SelectRandomItem();
            EnterEmail(email);
            EnterPhone(phone);
            EnterPassword(password);
            EnterConfirmPassword(password);
        }
    }
}
