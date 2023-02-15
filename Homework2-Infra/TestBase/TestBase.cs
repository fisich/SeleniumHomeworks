using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace Homework2_Infra.TestBase
{
    [TestFixture]
    public class TestBase : IDisposable
    {
        private const string GoogleApiHost = "https://chromedriver.storage.googleapis.com";
        private bool _disposed = false;
        public IWebDriver WebDriver { get; private set; }
        public WebDriverWait Waiter { get; private set; }

        [OneTimeSetUp]
        public void InitializeTestBase()
        {
            KillAllChromeDriverProcesses();
            //var latestVersion = GetLatestChromeDriverVersion();
            //DownloadChromeDriver(GoogleApiHost, latestVersion, AppContext.BaseDirectory, AppContext.BaseDirectory);
            //WebDriver = new ChromeDriver(AppContext.BaseDirectory);
            //WebDriver = new InternetExplorerDriver(@"C:\SeleniumDrivers\IEDriverServer.exe");
            WebDriver = new FirefoxDriver(@"C:\SeleniumDrivers\geckodriver.exe");
            Waiter = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(30));
            WebDriver.Manage().Window.Maximize();
        }

        [OneTimeTearDown]
        public void ClearTestBase() => Dispose();

        public static string GetLatestChromeDriverVersion()
        {
            var request = WebRequest.Create($"{GoogleApiHost}/LATEST_RELEASE");
            var proxy = WebRequest.GetSystemWebProxy();
            proxy.Credentials = CredentialCache.DefaultCredentials;
            request.Proxy = proxy;
            var response = request.GetResponse();
            using (var dataStream = response.GetResponseStream())
            {
                var reader = new StreamReader(dataStream);
                string latestVersion = reader.ReadToEnd();
                return latestVersion;
            }
        }

        public static void DownloadChromeDriver(string googleApisLink, string version, string destinationDir, string tempZipDir)
        {
            var pathToZip = Path.Combine(tempZipDir, "chrome.zip");
            var linkToChromeDriver = $"{googleApisLink}/{version}/chromedriver_win32.zip";
            var webClient = new WebClient();
            webClient.Headers["User-Agent"] =
                              "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.85 Safari/537.36";

            webClient.DownloadFile(linkToChromeDriver, pathToZip);

            using (ZipArchive source = ZipFile.Open(pathToZip, ZipArchiveMode.Read, null))
            {
                foreach (ZipArchiveEntry entry in source.Entries)
                {
                    string fullPath = Path.GetFullPath(Path.Combine(destinationDir, entry.FullName));

                    if (Path.GetFileName(fullPath).Length != 0)
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                        entry.ExtractToFile(fullPath, true);
                    }
                }
            }
            File.Delete(pathToZip);
        }

        public static void KillAllChromeDriverProcesses()
        {
            var processes = Process.GetProcessesByName("chromedriver");
            foreach (var process in processes)
            {
                process.Kill();
            }
        }

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
