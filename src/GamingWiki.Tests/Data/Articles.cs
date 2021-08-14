using System.Collections.Generic;
using System.Linq;
using GamingWiki.Models;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Web.Models.Articles;
using static GamingWiki.Tests.Data.Users;
using static GamingWiki.Tests.Data.Categories;

namespace GamingWiki.Tests.Data
{
    public static class Articles
    {
        private const int DefaultArticleId = 2;
        private const string DefaultHeading = "Test Heading";
        private const string DefaultContent = "Test content for articles";
        private const string DefaultPictureUrl =
            "https://techmonitor.ai/wp-content/uploads/sites/20/2016/06/what-is-URL.jpg";

        public static IEnumerable<Article> FiveArticles
            => Enumerable.Range(0, 5).Select(_ => new Article());

        public static Article TestArticle
            => new()
            {
                Id = DefaultArticleId,
                Author = TestUser
            };

        public static ArticleFormModel TestArticleFormModel
            => new()
            {
                Heading = DefaultHeading,
                Content = DefaultContent,
                PictureUrl = DefaultPictureUrl,
                CategoryId = TestCategory.Id
            };

        public static ArticleServiceEditModel TestValidArticleEditModel
            => new()
            {
                Heading = DefaultHeading,
                Content = DefaultContent,
                PictureUrl = DefaultPictureUrl
            };
        
        public static ArticleServiceEditModel TestInvalidArticleEditModel
            => new()
            {
                Heading = "a",
                Content = "b",
                PictureUrl = "c"
            };
    }
}
