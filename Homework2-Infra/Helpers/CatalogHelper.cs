using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Linq;

namespace Homework2_Infra.Helpers
{
    public class CatalogHelper
    {
        private readonly IWebDriver _driver;

        #region Simple actions and locators
        public void ClickAddProduct() => _driver.FindElement(By.CssSelector("td[id=content] a.button:nth-child(2)")).Click();
        public void ChangeTab(string tabName) => _driver.FindElements(By.CssSelector("ul.index a")).First(a => a.Text == tabName).Click();
        public void ClickSave() => _driver.FindElement(By.CssSelector("button[name=save]")).Click();
        #region General
        public void SetStatus(bool enabled)
        {
            if (enabled)
                _driver.FindElement(By.CssSelector("input[type=radio][value='1']")).Click();
            else
                _driver.FindElement(By.CssSelector("input[type=radio][value='0']")).Click();
        }
        public void SetName(string name) => _driver.FindElement(By.XPath("//input[@name='name[en]']")).SendKeys(name);
        public void SetCode(string code) => _driver.FindElement(By.CssSelector("input[name=code]")).SendKeys(code);
        public void SetCategory(string category)
        {
            var categories = _driver.FindElements(By.XPath("(//div[@class='input-wrapper'])[1]//tr"));
            categories.First(cat => cat.FindElements(By.XPath($".//td[contains(text(), '{category}')]")).Count != 0).FindElement(By.XPath("(.//td)[1]")).Click();
        }
        public void SelectDefaultCategory(string category) => new SelectElement(_driver.FindElement(By.CssSelector("select[name=default_category_id]"))).SelectByText(category);
        public void SelectProductGroups(string[] gendersToChoose)
        {
            var genders = _driver.FindElements(By.XPath("(//div[@class='input-wrapper'])[2]//tr")).Where(tr => tr.IsElementVisible(By.XPath("(.//td)[2]")));
            foreach (var gender in genders.Where(cat => gendersToChoose.Contains(cat.FindElement(By.XPath("(.//td)[2]")).Text)))
            {
                gender.FindElement(By.XPath("(.//td)[1]")).Click();
            }
        }
        public void SetQuantity(int quantity) {
            var el = _driver.FindElement(By.CssSelector("input[name=quantity]"));
            el.Clear();
            el.SendKeys(quantity.ToString());
        }
        public void SelectQuantityUnit(string quantityUnit) => new SelectElement(_driver.FindElement(By.CssSelector("select[name=quantity_unit_id]"))).SelectByText(quantityUnit);
        public void SelectDeliveryStatus(string deliveryStatus) => new SelectElement(_driver.FindElement(By.CssSelector("select[name=delivery_status_id]"))).SelectByText(deliveryStatus);
        public void SelectSoldOutStatus(string soldOutStatus) => new SelectElement(_driver.FindElement(By.CssSelector("select[name=sold_out_status_id]"))).SelectByText(soldOutStatus);
        public void SetDateValidFrom(string dateValidFrom) => (_driver as IJavaScriptExecutor).ExecuteScript($"document.querySelector('[name=date_valid_from]').value = '{dateValidFrom}'");
        public void SetDateValidTo(string dateValidTo) => (_driver as IJavaScriptExecutor).ExecuteScript($"document.querySelector('[name=date_valid_to]').value = '{dateValidTo}'");
        public void SetImage(string filePath) => _driver.FindElement(By.CssSelector("input[type=file]")).SendKeys(filePath);
        #endregion

        #region Information
        public void SelectManufacturer(string manufacturer) => new SelectElement(_driver.FindElement(By.CssSelector("select[name=manufacturer_id]"))).SelectByText(manufacturer);
        public void SelectSupplier(string supplier) => new SelectElement(_driver.FindElement(By.CssSelector("select[name=supplier_id]"))).SelectByText(supplier);
        public void SetKeywords(string keywords) => _driver.FindElement(By.CssSelector("input[name=keywords]")).SendKeys(keywords);
        public void SetShortDescription(string desription) => _driver.FindElement(By.XPath("//input[@name='short_description[en]']")).SendKeys(desription);
        public void SetDescription(string desription) => _driver.FindElement(By.CssSelector(".trumbowyg-editor")).SendKeys(desription);
        public void SetTitle(string title) => _driver.FindElement(By.XPath("//input[@name='head_title[en]']")).SendKeys(title);
        public void SetMetaDescription(string description) => _driver.FindElement(By.XPath("//input[@name='meta_description[en]']")).SendKeys(description);
        #endregion

        #region Prices
        public void SetPurchasePrice(float price, string currency)
        {
            var priceEl = _driver.FindElement(By.CssSelector("input[name=purchase_price]"));
            priceEl.Clear();
            priceEl.SendKeys(price.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));
            new SelectElement(_driver.FindElement(By.CssSelector("select[name=purchase_price_currency_code]"))).SelectByText(currency);
        }
        public void SetTaxPrice(float price, string currencyLocator) => _driver.FindElement(By.XPath($"//input[@name='prices[{currencyLocator}]']")).SendKeys(price.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));
        #endregion
        #endregion
        public CatalogHelper(IWebDriver driver)
        {
            _driver = driver;
        }
    }
}
