using System.Collections.Generic;
using GamingWiki.Web.Models.Areas;

namespace GamingWiki.Web.Models.Games
{
    public class GameListingModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Genre { get; set; }

        public string PictureUrl { get; set; }

        public double Rating { get; set; }

        public string TrailerUrl { get; set; }

        public string Area { get; set; }

        public IEnumerable<string> Creators { get; set; }

        public IDictionary<string, double> Ratings { get; set; }

        public IEnumerable<AreaViewModel> Areas { get; set; }
    }
}
