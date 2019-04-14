using System;
using System.Collections.Generic;
using System.Text;

namespace NewsProvider.Contracts
{
    public class NewsItem
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string Text { get; set; }
        public DateTime PublishDate { get; set; }
        public List<Category> Categories { get; set; }
        public string Url { get; set; }
        public Source Source { get; set; }

        public NewsItem()
        {
            Title = "";
            Image = "";
            Text = "";
            PublishDate = DateTime.Now;
            Categories = new List<Category>();
            Url = "";
            Source = new Source();
        }
    }
}
