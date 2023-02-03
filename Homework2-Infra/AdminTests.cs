using NUnit.Framework;
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
    }
}
