using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1_Infra
{
    class SoftwareTestingTests : TestBase.TestBase
    {
        [Test]
        public void CheckForumButtonTest()
        {
            WebDriver.Url = "https://software-testing.ru/";
            var forumButton = Waiter.Until<IWebElement>(driver =>
            {
                var button = WebDriver.FindElement(By.XPath("//span[@class='tpparenttitle' and text() = 'Форум']"));
                return (button.Displayed && button.Enabled) ? button : null;
            });
            forumButton.Click();
            var result = Waiter.Until<string>(driver =>
                {
                var title = WebDriver.Title;
                return title == "Форум тестировщиков" ? title : null;
                }
            );
            Assert.That(result, Is.EqualTo("Форум тестировщиков"));
        }
    }
}
