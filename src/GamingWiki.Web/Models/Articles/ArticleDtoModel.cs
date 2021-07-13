using System;

namespace GamingWiki.Web.Models.Articles
{
    public class ArticleDtoModel
    {
        public string Heading { get; set; }

        public string Content { get; set; }

        public int CategoryId { get; set; }

        public DateTime PublishedOn { get; set; }

        public string PictureUrl { get; set; }

        public string AuthorId { get; set; }
    }
}
