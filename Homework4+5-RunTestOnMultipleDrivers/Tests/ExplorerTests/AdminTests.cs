using NUnit.Framework;

namespace Homework4_5_RunTestOnMultipleDrivers.Tests.ExplorerTests
{
    public class AdminTests : TestBase.ExplorerTestBase
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