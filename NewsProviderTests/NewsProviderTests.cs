using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsProvider.Contracts;

namespace NewsProviderTests
{
    [TestClass]
    public class NewsProviderTests
    {
        [TestMethod]
        public void FetchNews()
        {
            NewsProvider.NewsProvider provider = new NewsProvider.NewsProvider();
            var news = provider.GetAllNews();
            Assert.IsInstanceOfType(news, typeof(IEnumerable<NewsItem>));
            Assert.IsFalse(news.Any());

            // No sources at the momenth.
            var sources = provider.GetSources();
            Assert.IsFalse(sources.Any());

            provider.AddRssSource("");
            news = provider.GetAllNews();
            Assert.IsFalse(news.Any());

            // No sources here also because no valid source added.
            sources = provider.GetSources();
            Assert.IsFalse(sources.Any());

            provider.AddRssSource("http://not.existing.url");
            news = provider.GetAllNews();
            Assert.IsFalse(news.Any());

            // No sources here also because no valid source added.
            sources = provider.GetSources();
            Assert.IsFalse(sources.Any());

            provider.AddRssSource("http://www.svd.se");
            news = provider.GetAllNews();
            Assert.IsFalse(news.Any());

            // No sources here also because no valid source added.
            sources = provider.GetSources();
            Assert.IsFalse(sources.Any());

            // Expressen does not have categories.
            provider.AddRssSource("https://feeds.expressen.se/nyheter/");
            news = provider.GetAllNews();
            Assert.IsTrue(news.Any());

            // One source should exist.
            sources = provider.GetSources();
            Assert.IsTrue(sources.Any());
            Assert.AreEqual(1, sources.Count());

            var categories = provider.GetCategories();
            Assert.IsFalse(categories.Any());

            // NT.se should have categories.
            provider.AddRssSource("https://www.nt.se/nyheter/norrkoping/rss/");
            news = provider.GetAllNews();
            Assert.IsTrue(news.Any());

            // Two sources should exist.
            sources = provider.GetSources();
            Assert.IsTrue(sources.Any());
            Assert.AreEqual(2, sources.Count());

            categories = provider.GetCategories();
            Assert.IsTrue(categories.Any());
        }
    }
}
