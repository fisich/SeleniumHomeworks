using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;

namespace Homework4_5_RunTestOnMultipleDrivers.TestBase
{
    [TestFixture]
    public class ExplorerTestBase : TestBase
    {
        private bool _disposed = false;
        public new IWebDriver WebDriver { get; private set; }

        [OneTimeSetUp]
        public override void InitializeTestBase()
        {
            WebDriver = new InternetExplorerDriver(@"C:\SeleniumDrivers\IEDriverServer.exe");
        }

        [TearDown]
        public override void ClearTestBase()
        {
            Dispose();
        }

        public override void Dispose()
        {
            if (_disposed)
                return;
            if (WebDriver != null)
            {
                WebDriver.Quit();
                WebDriver.Dispose();
                WebDriver = null;
            }
            _disposed = true;
        }
    }
}
