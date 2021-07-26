using System.Collections.Generic;
using GamingWiki.Services.Models.Comments;

namespace GamingWiki.Services.Models.Articles
{
    public class ArticleServiceDetailsModel
    {
        public int Id { get; set; }

        public string Author { get; set; }

        public string Heading { get; set; }

        public string Category { get; set; }

        public string Content { get; set; }

        public string PublishedOn { get; set; }

        public string PictureUrl { get; set; }

        public IEnumerable<CommentServiceModel> Comments { get; set; }
    }
}
