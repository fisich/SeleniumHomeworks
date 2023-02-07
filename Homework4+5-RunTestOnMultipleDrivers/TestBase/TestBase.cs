using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework4_5_RunTestOnMultipleDrivers.TestBase
{
    public abstract class TestBase : IDisposable
    {
        protected IWebDriver WebDriver { get; set; }
        protected WebDriverWait Waiter { get; set; }
        public abstract void InitializeTestBase();
        public abstract void ClearTestBase();
        public abstract void Dispose();
    }
}
