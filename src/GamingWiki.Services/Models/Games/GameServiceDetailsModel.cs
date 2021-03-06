using System.Collections.Generic;
using GamingWiki.Services.Models.Areas;
using GamingWiki.Services.Models.Characters;
using GamingWiki.Services.Models.Reviews;

namespace GamingWiki.Services.Models.Games
{
    public class GameServiceDetailsModel : GameServiceListingModel
    {
        public string Description { get; set; }

        public string Genre { get; set; }

        public double Rating { get; set; }

        public string TrailerUrl { get; set; }

        public string Area { get; set; }

        public int AreaId { get; set; }

        public bool IsApproved { get; set; }

        public string ContributorId { get; set; }

        public IEnumerable<string> Creators { get; set; }

        public IDictionary<string, double> Ratings { get; set; }

        public IEnumerable<AreaServiceModel> Areas { get; set; }

        public IEnumerable<CharacterGameServiceModel> Characters { get; set; }

        public IEnumerable<ReviewServiceSimpleModel> Reviews { get; set; }

        public IEnumerable<string> Platforms { get; set; }
    }
}
