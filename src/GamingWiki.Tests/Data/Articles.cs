using System.Collections.Generic;
using System.Linq;
using GamingWiki.Models;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Web.Models.Articles;
using static GamingWiki.Tests.Data.Users;
using static GamingWiki.Tests.Data.Categories;
using static GamingWiki.Tests.Common.TestConstants;

namespace GamingWiki.Tests.Data
{
    public static class Articles
    {
        public static IEnumerable<Article> FiveArticles
            => Enumerable.Range(0, 5).Select(_ => new Article());

        public static Article TestArticle
            => new()
            {
                Id = DefaultId,
                Author = TestUser
            };

        public static ArticleFormModel TestArticleFormModel
            => new()
            {
                Heading = DefaultHeading,
                Content = DefaultContent,
                PictureUrl = DefaultUrl,
                CategoryId = TestCategory.Id
            };

        public static ArticleServiceEditModel TestValidArticleEditModel
            => new()
            {
                Heading = DefaultHeading,
                Content = DefaultContent,
                PictureUrl = DefaultUrl
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
