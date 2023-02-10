using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework2_Infra.Helpers
{
    public static class MethodsExtensions
    {
        public static bool IsElementVisible(this IWebDriver webDriver, By by)
        {
            return webDriver.FindElements(by).Count > 0;
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
    }
}
