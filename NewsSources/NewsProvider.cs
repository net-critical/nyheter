using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NewsProvider.Contracts;
using NewsProvider.Sources;
using NewsProvider.Utils;

namespace NewsProvider
{
    /// <summary>
    /// Class that works with news feeds registered.
    /// </summary>
    public class NewsProvider
    {
        /// <summary>
        /// List of sources registered to work with.
        /// </summary>
        protected List<INewsSource> Sources;

        public NewsProvider()
        {
            Sources = new List<INewsSource>();
        }

        /// <summary>
        /// Add extra source to provider.
        /// </summary>
        /// <param name="source"></param>
        public void AddSource(INewsSource source)
        {
            Sources.Add(source);
        }

        /// <summary>
        /// Add Rss source to provider to work with.
        /// </summary>
        /// <param name="url">Rss feed url.</param>
        public void AddRssSource(string url)
        {
            AddSource(new RssSource(url));
        }

        /// <summary>
        /// Returns all news found in all registered news sources ordered by date published.
        /// </summary>
        /// <returns>List of news.</returns>
        public IEnumerable<NewsItem> GetAllNews()
        {
            List<NewsItem> items = new List<NewsItem>();

            foreach(INewsSource source in Sources)
            {
                items.AddRange(source.FetchAllItems());
            }

            return items.OrderByDescending(i => i.PublishDate);
        }

        /// <summary>
        /// Returns all news found in one specific news source registered.
        /// </summary>
        /// <param name="source">Source name or slug.</param>
        /// <returns>List of news filtered.</returns>
        public IEnumerable<NewsItem> GetNewsBySource(string source)
        {
            return GetAllNews().Where(i => i.Source.Slug == source.GenerateSlug()).ToList<NewsItem>();
        }

        /// <summary>
        /// Returns all news found in all sources registered within one specific category.
        /// </summary>
        /// <param name="category">Category name or slug.</param>
        /// <returns>List of news filtered.</returns>
        public IEnumerable<NewsItem> GetNewsByCategory(string category)
        {
            return GetAllNews().Where(i => i.Categories.Select(c => c.Slug).Contains(category.GenerateSlug())).ToList<NewsItem>();
        }

        /// <summary>
        /// Returns all categories found in all sources grouped by slug to filter similar ones and ordered by name.
        /// </summary>
        /// <returns>List of all categories.</returns>
        public IEnumerable<Category> GetCategories()
        {
            return GetAllNews().SelectMany(i => i.Categories).GroupBy(i => i.Slug).Select(i => i.First()).OrderBy(i => i.Name.ToLower()).ToList<Category>();
        }

        /// <summary>
        /// Returns all non empty sources registered ordered by name.
        /// </summary>
        /// <returns>List of sources.</returns>
        public IEnumerable<Source> GetSources()
        {
            return GetAllNews().Select(i => i.Source).GroupBy(i => i.Slug).Select(i => i.First()).OrderBy(i => i.Name.ToLower()).ToList<Source>();
        }
    }
}
