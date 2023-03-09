using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using MultiLevelArchitecture.PageObjects;

namespace MultiLevelArchitecture.TestBase
{
    [TestFixture]
    public class TestBase : IDisposable
    {
        private bool _disposed = false;
        public IWebDriver WebDriver { get; private set; }
        public WebDriverWait Waiter { get; private set; }
        public MainPage MainPageHelper { get; private set; }
        public ProductPage ProductPageHelper { get; private set; }
        public CheckoutPage CheckoutPageHelper { get; private set; }
        public CartObject Cart { get; private set; }

        [OneTimeSetUp]
        public void InitializeTestBase()
        {
            //WebDriver = new ChromeDriver(AppContext.BaseDirectory);
            //WebDriver = new InternetExplorerDriver(@"C:\SeleniumDrivers\IEDriverServer.exe");
            WebDriver = new FirefoxDriver(@"C:\SeleniumDrivers\geckodriver.exe");
            Waiter = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(30));
            WebDriver.Manage().Window.Maximize();
            MainPageHelper = new MainPage(WebDriver);
            ProductPageHelper = new ProductPage(WebDriver);
            CheckoutPageHelper = new CheckoutPage(WebDriver);
            Cart = new CartObject(WebDriver);
        }

        public void GoToUrl(string url)
        {
            WebDriver.Navigate().GoToUrl(url);
        }

        public void GoBack()
        {
            WebDriver.Navigate().Back();
        }

        [OneTimeTearDown]
        public void ClearTestBase() => Dispose();

        public void Dispose()
        {
            if (_disposed)
                return;
            WebDriver.Quit();
            WebDriver.Dispose();
            _disposed = true;
        }

    }
}
