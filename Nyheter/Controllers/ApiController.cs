using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NewsProvider.Contracts;

namespace Nyheter.Controllers
{
    /// <summary>
    /// The main API controler.
    /// Provides all data required to display news and related information on front-end application.
    /// Memory cache with 60s expiration time enabled on news returning endpoints.
    /// </summary>
    [Route("api")]
    public class ApiController : Controller
    {
        private readonly IMemoryCache _cache;

        private readonly int _cache_expiration_time;

        private readonly NewsProvider.NewsProvider _newsProvider;

        /// <summary>
        /// Registers few hardcoded news feeds to work with.
        /// </summary>
        /// <param name="memoryCache"></param>
        public ApiController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            _cache_expiration_time = 60;

            _newsProvider = new NewsProvider.NewsProvider();
            _newsProvider.AddRssSource("https://www.nt.se/nyheter/norrkoping/rss/");
            _newsProvider.AddRssSource("https://feeds.expressen.se/nyheter/");
            _newsProvider.AddRssSource("https://www.svd.se/?service=rss");
        }

        /// <summary>
        /// Provides all categories found in all feeds sorted by name.
        /// </summary>
        /// <returns>Returns JSON encoded categories.</returns>
        [HttpGet("categories")]
        public IActionResult Categories()
        {
            return new JsonResult(_newsProvider.GetCategories());
        }

        /// <summary>
        /// Provides all readable news sources registered sorted by name.
        /// </summary>
        /// <returns>Returns JSON encoded news sources.</returns>
        [HttpGet("sources")]
        public IActionResult Sources()
        {
            return new JsonResult(_newsProvider.GetSources());
        }

        /// <summary>
        /// Provides all news headlines found sorted by date published.
        /// </summary>
        /// <returns>Returns JSON encoded headlines.</returns>
        [HttpGet("news")]
        public IActionResult NewsAll()
        {
            string cacheKey = "_CACHE_NEWS_ALL_";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<NewsItem> items))
            {
                items = _newsProvider.GetAllNews();

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_cache_expiration_time));
                _cache.Set(cacheKey, items, cacheEntryOptions);
            }

            return new JsonResult(items);
        }

        /// <summary>
        /// Provides all news headlines found in one specific source provided and sorted by date published.
        /// </summary>
        /// <param name="source">Source name/slug to filter news.</param>
        /// <returns>Returns JSON encoded headlines.</returns>
        [HttpGet("news/source/{source}")]
        public IActionResult NewsBySource(string source)
        {
            string cacheKey = "_CACHE_NEWS_SRC_" + source.ToUpper();

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<NewsItem> items))
            {
                items = _newsProvider.GetNewsBySource(source);

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_cache_expiration_time));
                _cache.Set(cacheKey, items, cacheEntryOptions);
            }

            return new JsonResult(items);
        }

        /// <summary>
        /// Provides all news headlines found in one specific category provided and sorted by date published.
        /// </summary>
        /// <param name="category">Category name/slug to filter news.</param>
        /// <returns>Returns JSON encoded headlines.</returns>
        [HttpGet("news/category/{category}")]
        public IActionResult NewsByCategory(string category)
        {
            string cacheKey = "_CACHE_NEWS_CAT_" + category.ToUpper();

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<NewsItem> items))
            {
                items = _newsProvider.GetNewsByCategory(category);

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_cache_expiration_time));
                _cache.Set(cacheKey, items, cacheEntryOptions);
            }

            return new JsonResult(items);
        }
    }
}
