using System.Collections.Generic;
using GamingWiki.Services.Models.Games;
using GamingWiki.Services.Models.Genres;

namespace GamingWiki.Web.Models.Games
{
    public class GameFullModel
    {
        public PaginatedList<GameServiceListingModel> Games { get; set; }

        public IEnumerable<GenreServiceModel> Genres { get; set; }

        public KeyValuePair<object, object> Tokens { get; set; } = new();
    }
}
