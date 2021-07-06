using System;

namespace GamingWiki.Web.Models.Replies
{
    public class ReplyDtoModel
    {
        public string Content { get; set; }

        public int CommentId { get; set; }

        public string ReplierId { get; set; }

        public DateTime AddedOn { get; set; }
    }
}
