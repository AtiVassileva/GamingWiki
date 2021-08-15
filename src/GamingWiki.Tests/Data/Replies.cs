using GamingWiki.Models;
using GamingWiki.Web.Models.Replies;
using static GamingWiki.Tests.Common.TestConstants;
using static GamingWiki.Tests.Data.Comments;

namespace GamingWiki.Tests.Data
{
    public static class Replies
    {
        public static Reply TestReply => new()
        {
            Id = DefaultId,
            CommentId = TestComment.Id
        };

        public static ReplyFormModel TestValidReplyFormModel
            => new()
            {
                ReplyContent = DefaultContent
            };

        public static ReplyFormModel TestInvalidReplyFormModel
            => new()
            {
                ReplyContent = "a"
            };
    }
}
