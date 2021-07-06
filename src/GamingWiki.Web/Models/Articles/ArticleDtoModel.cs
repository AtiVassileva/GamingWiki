using System;
using GamingWiki.Models.Enums;

namespace GamingWiki.Web.Models.Articles
{
    public class ArticleDtoModel
    {
        public string Heading { get; set; }

        public string Content { get; set; }

        public Category Category { get; set; }

        public DateTime PublishedOn { get; set; }

        public string PictureUrl { get; set; }

        public string AuthorId { get; set; }
    }
}
