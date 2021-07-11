using System.ComponentModel;
using GamingWiki.Models.Enums;

namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class Game
    {
        private const double DefaultRating = 0.0;
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string PictureUrl { get; set; }

        public string TrailerUrl { get; set; }

        public int PlaceId { get; set; }

        public Place Place { get; set; }

        public GameClass Class { get; set; }

        public ICollection<Character> Characters { get; set; }
        = new HashSet<Character>();

        public ICollection<GameCreator> GamesCreators { get; set; }
            = new HashSet<GameCreator>();
    }
}
