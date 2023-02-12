using Homework2_Infra.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
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
        public ProductPageHelper ProductPage { get; private set; }

        [SetUp]
        public void Initialize()
        {
            MainPage = new MainPageHelper(WebDriver);
            ProductPage = new ProductPageHelper(WebDriver);
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

        [Test, Description("Task 10. Сheck that product information on the main page and product page are the same content and style")]
        public void CheckContentAndStyleOfProductInfoOnMainAndProductPagesTest()
        {
            WebDriver.Navigate().GoToUrl(MainPageHelper.BasePageUrl);
            var product = MainPage.GetAllProductCardsInsideInfoBlock(MainPage.GetInfoBlock("box-campaigns")).First();
            var (regularPriceElement, campaignPriceElement) = ProductPageHelper.GetPriceElementsInContext(product);
            var mainPageProductInfo = new Dictionary<string, string>
            {
                { "name", product.FindElement(By.CssSelector("[class=name]")).Text },
                { "regular-price", regularPriceElement.Text },
                { "campaign-price", campaignPriceElement?.Text },
            };
            var regularColor = MethodsExtensions.ParseColor(regularPriceElement.GetCssValue("color"));
            var campaignColor = MethodsExtensions.ParseColor(campaignPriceElement.GetCssValue("color"));
            Assert.That(regularPriceElement.TagName, Is.EqualTo("s"));
            Assert.IsTrue(regularColor.R == regularColor.G && regularColor.G == regularColor.B, "Main page regular price color is not grey");
            Assert.That(campaignPriceElement.TagName, Is.EqualTo("strong"));
            Assert.IsTrue(campaignColor.G == 0 && campaignColor.B == 0 && campaignColor.R > 0, "Main page sale price color is not red");
            Assert.IsTrue(Int32.Parse(regularPriceElement.Text.Replace("$", ""))
                > Int32.Parse(campaignPriceElement.Text.Replace("$", "")), "Sale price is higher then regular");
            product.Click();

            var (productRegularPriceElement, productCampaignPriceElement) = ProductPage.GetPriceElements();
            var productPageProductInfo = new Dictionary<string, string>
            {
                { "name", ProductPage.GetName() },
                { "regular-price", productRegularPriceElement.Text },
                { "campaign-price", productCampaignPriceElement.Text },
            };
            Assert.AreEqual(productPageProductInfo, mainPageProductInfo, "Info about product on main page and on product page are not the same");
            var productRegularColor = MethodsExtensions.ParseColor(productRegularPriceElement.GetCssValue("color"));
            var productCampaignColor = MethodsExtensions.ParseColor(productCampaignPriceElement.GetCssValue("color"));
            Assert.That(productRegularPriceElement.TagName, Is.EqualTo("s"));
            Assert.IsTrue(productRegularColor.R == productRegularColor.G && productRegularColor.G == productRegularColor.B, "Product page regular price color is not grey");
            Assert.That(productCampaignPriceElement.TagName, Is.EqualTo("strong"));
            Assert.IsTrue(productCampaignColor.G == 0 && productCampaignColor.B == 0 && productCampaignColor.R > 0, "Product page sale price color is not red");
            Assert.IsTrue(Int32.Parse(productRegularPriceElement.Text.Replace("$", "")) >
                Int32.Parse(productCampaignPriceElement.Text.Replace("$", "")), "Sale price is higher then regular");
        }
    }
}
