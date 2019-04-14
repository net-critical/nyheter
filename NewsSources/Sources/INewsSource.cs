using System;
using System.Collections.Generic;
using System.Text;
using NewsProvider.Contracts;

namespace NewsProvider.Sources
{
    public interface INewsSource
    {
        IEnumerable<NewsItem> FetchAllItems();
    }
}
