using System.Collections.Generic;
using GamingWiki.Web.Models.Replies;

namespace GamingWiki.Web.Models.Comments
{
    public class CommentListingModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string Commenter { get; set; }

        public IEnumerable<ReplyListingModel> Replies { get; set; }
    }
}
