using GamingWiki.Models.Enums;

namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Character
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PictureUrl { get; set; }

        public string Description { get; set; }

        public CharacterClass Class { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; }
    }
}
