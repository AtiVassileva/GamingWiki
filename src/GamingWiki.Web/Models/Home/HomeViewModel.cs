using System.Collections.Generic;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Services.Models.Games;

namespace GamingWiki.Web.Models.Home
{
    public class HomeViewModel
    {
        public IEnumerable<ArticleServiceHomeModel> LatestArticles { get; set; }

        public IEnumerable<GameServiceListingModel> BestGames { get; set; }
    }
}
