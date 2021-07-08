using System.Collections.Generic;

namespace GamingWiki.Web.Models.Games
{
    public class GameListingModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Class { get; set; }

        public string PictureUrl { get; set; }

        public string TrailerUrl { get; set; }

        public string Place { get; set; }

        public IEnumerable<string> Creators { get; set; }
    }
}
