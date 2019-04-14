using System;
using System.Collections.Generic;
using System.Text;
using NewsProvider.Utils;

namespace NewsProvider.Contracts
{
    public class Source
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug {
            get
            {
                return Name.GenerateSlug();
            }
        }

        public Source()
        {
            Name = "";
            Description = "";
        }
    }
}
