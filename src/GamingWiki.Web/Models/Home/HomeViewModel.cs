using System.Collections.Generic;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Services.Models.Games;
using GamingWiki.Services.Models.Tricks;

namespace GamingWiki.Web.Models.Home
{
    public class HomeViewModel
    {
        public IEnumerable<ArticleServiceHomeModel> LatestArticles { get; set; }

        public IEnumerable<GameServiceListingModel> LatestGames { get; set; }

        public IEnumerable<TrickServiceHomeModel> LatestTricks { get; set; }
    }
}
