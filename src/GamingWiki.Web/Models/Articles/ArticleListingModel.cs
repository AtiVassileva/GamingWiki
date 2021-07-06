using System.Collections.Generic;
using GamingWiki.Web.Models.Comments;

namespace GamingWiki.Web.Models.Articles
{
    public class ArticleListingModel
    {
        public int Id { get; set; }

        public string Author { get; set; }

        public string Heading { get; set; }

        public string Category { get; set; }

        public string Content { get; set; }

        public string PublishedOn { get; set; }

        public string PictureUrl { get; set; }

        public IEnumerable<CommentListingModel> Comments { get; set; }
    }
}
