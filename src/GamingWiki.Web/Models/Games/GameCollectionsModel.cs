using System.Collections.Generic;

namespace GamingWiki.Web.Models.Games
{
    public class GameCollectionsModel
    {
        public IEnumerable<string> PlaceTypes { get; set; }

        public IEnumerable<string> GameClasses { get; set; }
    }
}
