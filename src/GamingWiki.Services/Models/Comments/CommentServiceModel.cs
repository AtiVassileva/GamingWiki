using System.Collections.Generic;
using GamingWiki.Services.Models.Replies;

namespace GamingWiki.Services.Models.Comments
{
    public class CommentServiceModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string Commenter { get; set; }

        public string CommenterId { get; set; }

        public string AddedOn { get; set; }
        public IEnumerable<ReplyServiceModel> Replies { get; set; }
    }
}
