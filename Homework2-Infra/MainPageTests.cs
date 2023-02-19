using Homework2_Infra.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Homework2_Infra
{
    public class MainPageTests : TestBase.TestBase
    {
        public MainPageHelper MainPage { get; private set; }
        public ProductPageHelper ProductPage { get; private set; }
        public RegistrationHelper Registration { get; private set; }

        [SetUp]
        public void Initialize()
        {
            MainPage = new MainPageHelper(WebDriver);
            ProductPage = new ProductPageHelper(WebDriver);
            Registration = new RegistrationHelper(WebDriver); 
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
            Assert.IsTrue(Double.Parse(regularPriceElement.GetCssValue("font-size").Replace("px",""), CultureInfo.InvariantCulture)
                < Double.Parse(campaignPriceElement.GetCssValue("font-size").Replace("px", ""), CultureInfo.InvariantCulture),
                "Sale font size should be bigger than regular");
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
            Assert.IsTrue(Double.Parse(productRegularPriceElement.GetCssValue("font-size").Replace("px", ""), CultureInfo.InvariantCulture)
                < Double.Parse(productCampaignPriceElement.GetCssValue("font-size").Replace("px", ""), CultureInfo.InvariantCulture),
                "Sale font size should be bigger than regular");
        }

        [Test]
        public void RegisterNewUserTest()
        {
            WebDriver.Navigate().GoToUrl(MainPageHelper.BasePageUrl);
            MainPage.ClickRegisterNewUser();
            var email = $"{MethodsExtensions.RandomString(10).ToLower()}@{MethodsExtensions.RandomString(4).ToLower()}.ru";
            Registration.QuickFillInputForm("Alexey","Fisenko", MethodsExtensions.RandomString(20), new Random().Next(10000, 99999).ToString(),
                "Moscow", "United States", email, MethodsExtensions.RandomString(12, true), "password");
            Registration.ClickCreateAccount();
            WebDriver.WaitElementEnabled(MainPageHelper.LogoutBy); // wait after redirect
            MainPage.ClickLogout();
            MainPage.Login(email, "password");
            WebDriver.WaitElementEnabled(MainPageHelper.LogoutBy); // wait after redirect
            MainPage.ClickLogout();
        }

        [Test, Description("Task 13. Add 3 products to cart, then delete them one by one")]
        public void AddAndRemoveProductsInCartTest()
        {
            WebDriver.Navigate().GoToUrl(MainPageHelper.BasePageUrl);
            var cartQuantity = Int32.Parse(MainPage.GetCartQuantityElement().Text);
            while (cartQuantity < 3)
            {
                if (WebDriver.WaitElementVisible(By.CssSelector($"div#box-most-popular")) == null)
                    throw new ApplicationException("Most popular title not found on main page");
                var firstProduct = MainPage.GetAllProductCards().First();
                firstProduct.Click();
                if (WebDriver.WaitElementVisible(By.CssSelector("h1.title")) == null)
                    throw new ApplicationException("Product page not loaded or h1 header not found");
                var quantityTextBeforeAddToCart = MainPage.GetCartQuantityElement().Text;
                ProductPage.SelectFirstSizeIfOptionExists();
                ProductPage.ClickAddToCart();
                if (!MainPage.GetCartQuantityElement().WaitElementUpdateCurrentText(quantityTextBeforeAddToCart))
                    throw new ApplicationException("Cart info was not updated after add product");
                WebDriver.Navigate().Back();
                cartQuantity = Int32.Parse(MainPage.GetCartQuantityElement().Text);
            }
            MainPage.ClickCheckout();
            var productRowsCount = MainPage.GetOrderSummaryRows().Count;
            while (productRowsCount != 0)
            {
                var button = MainPage.GetRemoveCartItemButton();
                button.Click();
                var waitTableUpdate = Task.Run(() =>
                {
                    while (MainPage.GetOrderSummaryRows().Count == productRowsCount)
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                }, new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token);
                waitTableUpdate.Wait();
                if (!waitTableUpdate.IsCompleted)
                    throw new ApplicationException("Cart summary table not updated after remove product");
                else
                    productRowsCount = MainPage.GetOrderSummaryRows().Count;
            }
            Assert.That(MainPage.GetCartMessage(), Is.EqualTo("There are no items in your cart."));
        }
    }
}
