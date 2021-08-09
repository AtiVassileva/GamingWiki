using GamingWiki.Services.Models.Articles;

namespace GamingWiki.Web.Infrastructure
{
    public static class ModelExtensions
    {
        public static string GetInformation(this
            ArticleServiceDetailsModel article) 
            => article.Heading + "-" + article.PublishedOn;
    }
}
