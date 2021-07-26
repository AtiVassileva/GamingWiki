using System.Collections.Generic;
using GamingWiki.Services.Models.Games;
using GamingWiki.Services.Models.Genres;

namespace GamingWiki.Web.Models.Games
{
    public class GameFullModel
    {
        public IEnumerable<GameServiceListingModel> Games { get; set; }

        public IEnumerable<GenreServiceModel> Genres { get; set; }
    }
}
