using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsProvider.Utils;

namespace NewsProviderTests
{
    [TestClass]
    public class HtmlHelperTests
    {
        [TestMethod]
        public void ImageExists()
        {
            string html = "<img src=\"https://example.com/assets/my-image.large.png\"/>";
            Assert.IsNotNull(html.ExtractImage());

            html = "<img src=\"https://example.com/assets/my-image.large.png\" /> <img attr=\"foo\" src=\"https://example.com/assets/my-image.large.png\"/>";
            Assert.IsNotNull(html.ExtractImage());
        }

        [TestMethod]
        public void ImageDoesNotExist()
        {
            string html = "";
            Assert.IsNull(html.ExtractImage());

            html = "<image src=\"https://example.com/assets/my-image.large.png\" /> <tag attr=\"foo\" src=\"https://example.com/assets/my-image.large.png\"/>";
            Assert.IsNull(html.ExtractImage());
        }

        [TestMethod]
        public void HtmlToText()
        {
            string html = @"<li class=""media - list__item media - list__item--3"">
                    < div class=""media media--icon block-link"">
                    <div class=""media__image"">
                        <div class=""responsive-image""><img src = ""https://ichef.bbc.co.uk/wwhp/624/cpsprodpb/4FD2/production/_106443402_p076frg4.jpg"" class=""image-replace"" alt=""The 'world's biggest aeroplane'"" data-src=""https://ichef.bbc.co.uk/wwhp/{width}/cpsprodpb/4FD2/production/_106443402_p076frg4.jpg"" width=""304""></div>                </div>

                                        <span class=""media__icon icon icon--video"" aria-hidden=""true""></span>
                
                    <div class=""media__content"">

                                                <h3 class=""media__title"">
                                <a class=""media__link"" href=""/news/world-us-canada-47924204"" rev=""news|headline"">
                                                                        'World's largest plane' takes to the air                                                            </a>
                            </h3>
                    
                                                <p class=""media__summary"">
                                                                Its wingspan measures 117m - the length of an American football field.                                                    </p>
                    
                                                <a class=""media__tag tag tag--news"" href=""/news/world/us_and_canada"" rev=""news|source"">US &amp; Canada</a>
                    
                    
                    </div>

                    <a class=""block-link__overlay-link"" href=""/news/world-us-canada-47924204"" rev=""news|overlay"" tabindex=""-1"" aria-hidden=""true"">
                        'World's largest plane' takes to the air                </a>
                </div>

            </li>";

            Assert.IsFalse(html.ExtractText().Contains(">") || html.ExtractText().Contains("<"));
        }

        [TestMethod]
        public void TextToSlug()
        {
            string text = "";
            Assert.AreEqual("", text.GenerateSlug());

            text = "Så här överlistar du ditt köpsug";
            Assert.AreEqual("sa-har-overlistar-du-ditt-kopsug", text.GenerateSlug());

            text = "Lietuviškos raidės nepalaikomos";
            Assert.AreEqual("lietuvikos-raids-nepalaikomos", text.GenerateSlug());

            text = "Multiple  spaces     works!";
            Assert.AreEqual("multiple-spaces-works", text.GenerateSlug());
        }

        [TestMethod]
        public void ParseDate()
        {
            string date = "";
            Assert.AreEqual(DateTime.Now.ToShortDateString(), date.ParseDate().ToShortDateString());

            date = "2019-12-12 12:12:12";
            Assert.AreEqual(new DateTime(2019, 12, 12, 12, 12, 12), date.ParseDate());

            date = "2019-13-12 12:12:12";
            Assert.AreEqual(DateTime.Now.ToShortDateString(), date.ParseDate().ToShortDateString());
        }
    }
}
