using System.Collections.Generic;
using GamingWiki.Services.Models.Games;
using GamingWiki.Services.Models.Genres;

namespace GamingWiki.Web.Models.Games
{
    public class GameFullModel : BaseFullModel
    {
        public PaginatedList<GameServiceListingModel> Games { get; set; }

        public IEnumerable<GenreServiceModel> Genres { get; set; }
    }
}
