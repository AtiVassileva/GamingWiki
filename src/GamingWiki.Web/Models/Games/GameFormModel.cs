using System.ComponentModel.DataAnnotations;
using static GamingWiki.Web.Common.DataConstants;

namespace GamingWiki.Web.Models.Games
{
    public class GameFormModel
    {
        [Required]
        [MinLength(DefaultMinLength)]
        [MaxLength(DefaultMaxLength)]
        public string Name { get; set; }

        [Url]
        [Required]
        public string PictureUrl { get; set; }

        [Required]
        [MinLength(DescriptionMinLength)]
        public string Description { get; set; }

        [Required]
        public string CreatorsNames { get; set; }

        [Required]
        [MinLength(DefaultMinLength)]
        [RegularExpression(ValidPlaceNameRegex)]
        public string PlaceName { get; set; }

        [Required]
        public string PlaceType { get; set; }
    }
}
