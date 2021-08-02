using System.Collections.Generic;
using GamingWiki.Services.Models.Comments;

namespace GamingWiki.Services.Models.Articles
{
    public class ArticleServiceDetailsModel : ArticleAllServiceModel
    {
        public string Author { get; set; }

        public string AuthorId { get; set; }

        public string Content { get; set; }

        public IEnumerable<CommentServiceModel> Comments { get; set; }
    }
}
