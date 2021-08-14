using System;
using GamingWiki.Models;
using GamingWiki.Web.Models.Comments;
using static GamingWiki.Tests.Data.Articles;
using static GamingWiki.Tests.Data.Users;
using static GamingWiki.Tests.Common.TestConstants;

namespace GamingWiki.Tests.Data
{
    public class Comments
    {
        public static CommentFormModel TestValidCommentFormModel =>
            new()
            {
                Content = DefaultContent
            };

        public static CommentFormModel TestInvalidCommentFormModel =>
            new()
            {
                Content = "a"
            };

        public static Comment TestComment =>
            new()
            {
                Id = DefaultId,
                ArticleId = TestArticle.Id,
                CommenterId = TestUser.Id,
                Content = DefaultContent,
                AddedOn = DateTime.UtcNow
            };
    }
}
