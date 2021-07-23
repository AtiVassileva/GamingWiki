using System.Collections.Generic;
using GamingWiki.Web.Models.Genres;

namespace GamingWiki.Web.Models.Games
{
    public class GameFullModel
    {
        public IEnumerable<GameViewModel> Games { get; set; }

        public IEnumerable<GenreViewModel> Genres { get; set; }
    }
}
