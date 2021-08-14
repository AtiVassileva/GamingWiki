using System;
using GamingWiki.Models;
using GamingWiki.Web.Models.Comments;
using static GamingWiki.Tests.Data.Articles;
using static GamingWiki.Tests.Data.Users;

namespace GamingWiki.Tests.Data
{
    public class Comments
    {
        private const string DefaultContent = "Test content for articles";
        private const int DefaultId = 9;

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
