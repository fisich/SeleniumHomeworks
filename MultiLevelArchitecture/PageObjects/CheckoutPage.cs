using MultiLevelArchitecture.Helpers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiLevelArchitecture.PageObjects
{
    public class CheckoutPage
    {
        private readonly IWebDriver _driver;

        #region Simple actions and locators
        public IReadOnlyCollection<IWebElement> GetOrderSummaryRows()
        {
            var allRows = _driver.FindElements(By.CssSelector("table.dataTable.rounded-corners tr"));
            return allRows.Take(allRows.Count - 4).Skip(1).ToList();
        }

        public string GetCheckoutMessage() => _driver.FindElement(By.CssSelector("div#content em")).Text;

        public void RemoveProductFromCart()
        {
            int productRowsCountBeforeRemove = GetOrderSummaryRows().Count;
            _driver.WaitElementEnabled(By.CssSelector("button[name=remove_cart_item]")).Click();
            WaitUntilProductCartChanged(productRowsCountBeforeRemove);
        }
        #endregion

        public CheckoutPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void WaitUntilProductCartChanged(int fromCount)
        {
            var waitTableUpdate = Task.Run(() =>
            {
                while (GetOrderSummaryRows().Count == fromCount)
                    Thread.Sleep(TimeSpan.FromSeconds(1));
            }, new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token);
            waitTableUpdate.Wait();
            if (!waitTableUpdate.IsCompleted)
                throw new ApplicationException("Cart summary table not updated after remove product");
        }
    }
}
