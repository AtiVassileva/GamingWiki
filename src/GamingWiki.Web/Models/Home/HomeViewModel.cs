using System.Collections.Generic;
using GamingWiki.Web.Models.Articles;
using GamingWiki.Web.Models.Games;

namespace GamingWiki.Web.Models.Home
{
    public class HomeViewModel
    {
        public IEnumerable<ArticleHomeModel> LatestArticles { get; set; }

        public IEnumerable<GameHomeModel> BestGames { get; set; }
    }
}
