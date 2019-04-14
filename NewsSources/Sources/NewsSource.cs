using System;
using System.Collections.Generic;
using System.Text;
using NewsProvider.Contracts;

namespace NewsProvider.Sources
{
    public abstract class NewsSource : INewsSource
    {
        protected string SourceUrl { get; set; }

        public NewsSource(string sourceUrl)
        {
            this.SourceUrl = sourceUrl;
        }

        public abstract IEnumerable<NewsItem> FetchAllItems();
    }
}
