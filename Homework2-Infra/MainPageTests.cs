using Homework2_Infra.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework2_Infra
{
    public class MainPageTests : TestBase.TestBase
    {
        public MainPageHelper MainPage { get; private set; }

        [SetUp]
        public void Initialize()
        {
            MainPage = new MainPageHelper(WebDriver);
        }

        [Test]
        public void CheckThatEveryProductHaveOnlyOneStickerTest()
        {
            WebDriver.Navigate().GoToUrl(MainPageHelper.BasePageUrl);
            var cards = MainPage.GetAllProductCards();
            foreach (var card in cards)
            {
                Assert.That(MainPage.GetAllStickersInElement(card).Count, Is.EqualTo(1), $"Card {card.Text} has less/more than one sticker");
            }
        }
    }
}
