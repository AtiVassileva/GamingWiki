namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using static Common.DataConstants;

    public class Game
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(GameNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string PictureUrl { get; set; }

        [Required]
        public string TrailerUrl { get; set; }

        public int AreaId { get; set; }

        public Area Area { get; set; }

        public int GenreId { get; set; }

        public Genre Genre { get; set; }

        public ICollection<Character> Characters { get; set; }
        = new HashSet<Character>();

        public ICollection<GameCreator> GamesCreators { get; set; }
            = new HashSet<GameCreator>();
    }
}
