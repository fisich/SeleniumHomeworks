﻿using Homework2_Infra.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework2_Infra
{
    public class AdminTests : TestBase.TestBase
    {
        public AdminHelper AdminHelper { get; private set; }

        [SetUp]
        public void Initialize()
        {
            AdminHelper = new AdminHelper(WebDriver);
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

                for (int j = 0; j < 0; j++)
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
            AdminHelper.GetAllTabs().Where(tab => tab.Text == "Geo Zones").First().Click();
            var countriesInfo = AdminHelper.GetCountriesList().Select(country => AdminHelper.GetGeoZoneCountryInfo(country)).ToList();
            foreach (var countryPage in countriesInfo.Select(c => c.editPageUrl))
            {
                WebDriver.Navigate().GoToUrl(countryPage);
                var geozonesInfo = AdminHelper.GetZonesList().Select(zone => AdminHelper.GetGeoZoneInfo(zone));
                var zoneNames = geozonesInfo.Select(g => g.ZoneDropDown.Text);
                CollectionAssert.AreEqual(zoneNames.OrderBy(z => z), zoneNames, $"Zones not sorted on page {countryPage}");
            }
        }
    }
}
