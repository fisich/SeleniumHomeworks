using Homework2_Infra.Helpers;
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
    }
}
