using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using NewsProvider.Contracts;
using NewsProvider.Utils;

namespace NewsProvider.Sources
{
    public class RssSource : NewsSource
    {
        public RssSource(string sourceUrl) : base(sourceUrl) { }

        /// <summary>
        /// Fetch all news headlines found in RSS feed.
        /// </summary>
        /// <returns>List of headlines found or empty list on error.</returns>
        public override IEnumerable<NewsItem> FetchAllItems()
        {
            try
            {
                XDocument doc = XDocument.Load(SourceUrl);

                var eChannel = doc.Root.Descendants().First(i => i.Name.LocalName == "channel");

                // Fetch feed title and description.
                var source = new Source()
                {
                    Name = eChannel.Elements().FirstOrDefault(i => i.Name.LocalName == "title")?.Value ?? "",
                    Description = eChannel.Elements().FirstOrDefault(i => i.Name.LocalName == "description")?.Value ?? "",
                };

                // Go through all items in the feed and fetch all data required.
                var items = from item in eChannel.Elements().Where(i => i.Name.LocalName == "item")
                    select new NewsItem
                    {
                        Title = item.Elements().FirstOrDefault(i => i.Name.LocalName == "title")?.Value ?? "",
                        Text = item.Elements().FirstOrDefault(i => i.Name.LocalName == "description")?.Value.ExtractText() ?? "",
                        Image = item.Elements().FirstOrDefault(i => i.Name.LocalName == "description")?.Value.ExtractImage() ?? "",
                        PublishDate = item.Elements().FirstOrDefault(i => i.Name.LocalName == "pubDate")?.Value.ParseDate() ?? DateTime.Now,
                        Categories = item.Elements().Where(i => i.Name.LocalName == "category").Select(i => new Category()
                        {
                            Name = i.Value,
                        }).ToList<Category>(),
                        Url = item.Elements().FirstOrDefault(i => i.Name.LocalName == "link")?.Value ?? "",
                        Source = source,
                    };

                return items;
            }
            catch
            {
                return new List<NewsItem>();
            }
        }
    }
}
