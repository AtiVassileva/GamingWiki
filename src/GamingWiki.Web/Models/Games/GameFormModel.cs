using System.ComponentModel.DataAnnotations;

namespace GamingWiki.Web.Models.Games
{
    public class GameFormModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Url]
        public string PictureUrl { get; set; }

        [Required]
        [MinLength(10)]
        public string Description { get; set; }

        [Required]
        public string CreatorsNames { get; set; }

        [Required]
        [MinLength(3)]
        [RegularExpression(@"[A-Za-z\s\.]+")]
        public string PlaceName { get; set; }

        [Required]
        public string PlaceType { get; set; }
    }
}
