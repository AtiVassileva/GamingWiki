using System;

namespace GamingWiki.Web.Models.Comments
{
    public class CommentDtoModel
    {
        public string Content { get; set; }

        public int ArticleId { get; set; }

        public string CommenterId { get; set; }

        public DateTime AddedOn { get; set; }
    }
}
