using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MultiLevelArchitecture.Helpers
{
    public static class MethodsExtensions
    {
        public static string RandomString(int length, bool digitsOnly = false)
        {
            const string digits = "0123456789";
            const string chars = "abcdefghijklmopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" + digits;
            var random = new Random();
            if (digitsOnly)
                return new string(Enumerable.Range(1, length).Select(_ => digits[random.Next(digits.Length)]).ToArray());
            else
                return new string(Enumerable.Range(1, length).Select(_ => chars[random.Next(chars.Length)]).ToArray());
        }

        public static bool IsElementVisible(this IWebDriver webDriver, By by)
        {
            return webDriver.FindElements(by).Count > 0;
        }

        public static bool IsElementVisible(this IWebElement webElement, By by)
        {
            return webElement.FindElements(by).Count > 0;
        }

        public static bool IsElementEnabled(this IWebDriver webDriver, By by)
        {
            if (webDriver.IsElementVisible(by))
            {
                return webDriver.FindElement(by).Enabled;
            }
            else
                return false;
        }

        public static IWebElement WaitElementVisible(this IWebDriver webDriver, By by)
        {
            var waiter = new WebDriverWait(webDriver, TimeSpan.FromSeconds(30));
            var isVisible = waiter.Until<bool>(_ => webDriver.IsElementVisible(by));
            if (isVisible)
                return webDriver.FindElement(by);
            else
                return null;
        }

        public static IWebElement WaitElementEnabled(this IWebDriver webDriver, By by)
        {
            var waiter = new WebDriverWait(webDriver, TimeSpan.FromSeconds(30));
            var isEnabled = waiter.Until<bool>(_ => webDriver.IsElementEnabled(by));
            if (isEnabled)
                return webDriver.FindElement(by);
            else
                return null;
        }

        public static bool WaitElementUpdateCurrentText(this IWebElement webElement, string currentText)
        {
            var task = Task.Run(() =>
            {
                while (webElement.Text == currentText)
                    Thread.Sleep(TimeSpan.FromSeconds(1));
            }, new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token);
            task.Wait();
            return task.IsCompleted && !task.IsCanceled;
        }

        public static void SelectRandomItem(this SelectElement select)
        {
            select.SelectByIndex(new Random().Next(0,select.Options.Count));
        }

        public static Color ParseColor(string rawColor)
        {
            if (rawColor.StartsWith("rgba")) // Chrome and explorer
            {
                var rgba = Regex.Replace(rawColor, @"\brgba\(\b|\s|\)", "")
                    .Split(',')
                    .Select(s => Int32.Parse(s))
                    .ToArray();
                return Color.FromArgb(rgba[3], rgba[0], rgba[1], rgba[2]);
            }
            else if(rawColor.StartsWith("rgb")) // Firefox
            {
                var rgb = Regex.Replace(rawColor, @"\brgb\(|\s|\)", "")
                    .Split(',')
                    .Select(s => Int32.Parse(s))
                    .ToArray();
                return Color.FromArgb(1, rgb[0], rgb[1], rgb[2]);
            }
            else
            {
                throw new NotImplementedException($"Unsupported color format {rawColor}");
            }
        }

        public static string WaitOtherWindowAppears(this IWebDriver driver, params string[] handledWindows)
        {
            var task = Task<string>.Run(() =>
            {
                while (driver.WindowHandles.Equals(handledWindows))
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
                if (driver.WindowHandles.Equals(handledWindows))
                    return null;
                else
                {
                    return driver.WindowHandles.Except(handledWindows).First();
                }
            }, new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token);
            task.Wait();
            return task.Result;
        }
    }
}
