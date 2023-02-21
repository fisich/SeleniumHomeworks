using Homework2_Infra.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Linq;

namespace Homework2_Infra
{
    public class AdminTests : TestBase.TestBase
    {
        public AdminHelper AdminHelper { get; private set; }
        public CatalogHelper CatalogHelper { get; private set; }
        public CountryHelper CountryHelper { get; private set; }

        [SetUp]
        public void Initialize()
        {
            AdminHelper = new AdminHelper(WebDriver);
            CatalogHelper = new CatalogHelper(WebDriver);
            CountryHelper = new CountryHelper(WebDriver);
        }

        [Test]
        public void LoginAsAdminTest()
        {
            WebDriver.Navigate().GoToUrl(AdminHelper.BasePageUrl);
            Assert.That(WebDriver.Url, Is.EqualTo("http://localhost/litecart/admin/login.php?redirect_url=%2Flitecart%2Fadmin%2F"));
            AdminHelper.LoginAsAdmin("admin", "admin");
            Assert.That(WebDriver.Url, Is.EqualTo(AdminHelper.BasePageUrl));
        }

        [Test, Description("Check that all left tabs and subtabs have h1 header")]
        public void CheckH1HeaderOnTabsTest()
        {
            WebDriver.Navigate().GoToUrl(AdminHelper.BasePageUrl);
            AdminHelper.LoginAsAdmin("admin", "admin");
            var tabsCount = AdminHelper.GetAllTabs().Count;
            for (int i = 0; i < tabsCount; i++)
            {
                var tab = AdminHelper.GetAllTabs().ElementAt(i);
                var tabText = tab.Text;
                tab.Click();
                var header = WebDriver.WaitElementVisible(AdminHelper.ByTabHeader());
                Assert.NotNull(header, $"Can't found H1 header on tab: {tabText}");
                var subTabsCount = AdminHelper.GetAllSubTabs().Count;
                for (int j = 0; j < subTabsCount; j++)
                {
                    var subtab = AdminHelper.GetAllSubTabs().ElementAt(j);
                    var subTabText = subtab.Text;
                    subtab.Click();
                    var subheader = WebDriver.WaitElementVisible(AdminHelper.ByTabHeader());
                    Assert.NotNull(subheader, $"Can't found H1 header on tab: {subTabText}");
                }
            }
        }

        [Test]
        public void CheckThatCountriesAndZonesSortedByNames()
        {
            WebDriver.Navigate().GoToUrl(AdminHelper.BasePageUrl);
            AdminHelper.LoginAsAdmin("admin", "admin");
            AdminHelper.GetAllTabs().Where(tab => tab.Text == "Countries").First().Click();
            var countriesInfo = AdminHelper.GetCountriesList().Select(country => AdminHelper.GetCountryInfo(country)).ToList();
            var countriesNames = countriesInfo.Select(c => c.Name);
            CollectionAssert.AreEqual(countriesNames.OrderBy(c => c), countriesNames, "Countries list not sorted by name");
            var countryEditPages = countriesInfo.Where(c => c.Zones > 0).Select(c => c.editPageUrl);
            foreach (var countryPage in countryEditPages)
            {
                WebDriver.Navigate().GoToUrl(countryPage);
                var zonesInfo = AdminHelper.GetZonesList().Select(zone => AdminHelper.GetZoneInfo(zone));
                var zoneNames = zonesInfo.Select(z => z.Name);
                CollectionAssert.AreEqual(zoneNames.OrderBy(z => z), zoneNames, $"Zones not sorted on page {countryPage}");
            }
        }

        [Test]
        public void CheckThatGeoZonesSortedByNames()
        {
            WebDriver.Navigate().GoToUrl(AdminHelper.BasePageUrl);
            AdminHelper.LoginAsAdmin("admin", "admin");
            AdminHelper.GetAllTabs().First(tab => tab.Text == "Geo Zones").Click();
            var countriesInfo = AdminHelper.GetCountriesList().Select(country => AdminHelper.GetGeoZoneCountryInfo(country)).ToList();
            foreach (var countryPage in countriesInfo.Select(c => c.editPageUrl))
            {
                WebDriver.Navigate().GoToUrl(countryPage);
                var geozonesInfo = AdminHelper.GetZonesList().Select(zone => AdminHelper.GetGeoZoneInfo(zone));
                var zoneNames = geozonesInfo.Select(g => new SelectElement(g.ZoneDropDown).SelectedOption.Text).ToList();
                CollectionAssert.AreEqual(zoneNames.OrderBy(z => z), zoneNames, $"Zones not sorted on page {countryPage}");
            }
        }

        [Test, Description("Task 12. Add product, fill General, Information and Prices. Check save in admin panel.")]
        public void AddProductToCatalogTest()
        {
            WebDriver.Navigate().GoToUrl(AdminHelper.BasePageUrl);
            AdminHelper.LoginAsAdmin("admin", "admin");
            AdminHelper.GetAllTabs().First(tab => tab.Text == "Catalog").Click();
            CatalogHelper.ClickAddProduct();
            // Fill General tab
            CatalogHelper.SetStatus(true);
            var name = $"Duck_{MethodsExtensions.RandomString(3, true)}";
            CatalogHelper.SetName(name);
            CatalogHelper.SetCode(MethodsExtensions.RandomString(6).ToUpper());
            CatalogHelper.SetCategory("Rubber Ducks");
            CatalogHelper.SelectDefaultCategory("Rubber Ducks");
            CatalogHelper.SelectProductGroups(new[] { "Female", "Unisex" });
            CatalogHelper.SetQuantity(5);
            CatalogHelper.SelectSoldOutStatus("Temporary sold out");
            var imagePath = Path.Combine(Environment.CurrentDirectory, @"Resources\donald-duck.png");
            CatalogHelper.SetImage(imagePath);
            CatalogHelper.SetDateValidFrom("2023-02-18");
            CatalogHelper.SetDateValidTo("2024-02-18");
            // Fill Information tab
            CatalogHelper.ChangeTab("Information");
            if (WebDriver.WaitElementVisible(By.CssSelector("div#tab-information")) == null)
                throw new ApplicationException("Information tab is not visible or not found");
            CatalogHelper.SelectManufacturer("ACME Corp.");
            // Keep default supplier
            CatalogHelper.SetKeywords("Donald duck, disney");
            CatalogHelper.SetShortDescription("This is short description of Donald Duck");
            CatalogHelper.SetDescription($"Text about Donald Duck{Environment.NewLine}This is Walt Disney character");
            CatalogHelper.SetTitle("DISNEY");
            CatalogHelper.SetMetaDescription("This is meta description text");
            // Fill price tab
            CatalogHelper.ChangeTab("Prices");
            if (WebDriver.WaitElementVisible(By.CssSelector("div#tab-prices")) == null)
                throw new ApplicationException("Prices tab is not visible or not found");
            CatalogHelper.SetPurchasePrice(10, "US Dollars");
            // Keep default tax class
            CatalogHelper.SetTaxPrice(1, "USD");
            CatalogHelper.SetTaxPrice(0.5f, "EUR");
            CatalogHelper.ClickSave();
            if (WebDriver.WaitElementVisible(AdminHelper.ByTabHeader()) == null)
                throw new ApplicationException("Catalog page is not loaded or header not found");
            var catalogNames = WebDriver.FindElements(By.CssSelector("table.dataTable td:nth-child(3)"));
            Console.WriteLine($"Saved catalogs: {String.Join(", ", catalogNames.Select(c => c.Text))}");

            Assert.IsTrue(catalogNames.Any(c => c.Text == name), $"Duck {name} wasn't saved");
        }

        [Test, Description("Task 14. Open add new country, then check that all info links opened in new window on click")]
        public void CheckThatLinksOnAddCountryPageOpenedInNewWindow()
        {
            WebDriver.Navigate().GoToUrl(AdminHelper.BasePageUrl);
            AdminHelper.LoginAsAdmin("admin", "admin");
            AdminHelper.GetAllTabs().First(tab => tab.Text == "Countries").Click();
            CountryHelper.ClickAddCountry();
            var links = CountryHelper.GetExternalLinks();
            foreach (var link in links)
            {
                var currentWindow = WebDriver.CurrentWindowHandle;
                var currentTitle = WebDriver.Title;
                link.Click();
                var newWindow = WebDriver.WaitOtherWindowAppears(currentWindow);
                if (newWindow == null)
                    throw new ApplicationException("New window link not appear");
                WebDriver.SwitchTo().Window(newWindow);
                Assert.That(WebDriver.Title, Is.Not.EqualTo(currentTitle), "Title not changed");
                WebDriver.Close();
                WebDriver.SwitchTo().Window(currentWindow);
            }
            Assert.That(WebDriver.WindowHandles.Count, Is.EqualTo(1));
        }
    }
}
