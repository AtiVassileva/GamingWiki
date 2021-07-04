using System.ComponentModel.DataAnnotations;
using static GamingWiki.Web.Common.DataConstants;

namespace GamingWiki.Web.Models.Characters
{
    public class CharacterFormModel
    {
        [Required]
        [MinLength(DefaultMinLength)]
        [MaxLength(DefaultMaxLength)]
        public string Name { get; set; }

        [Url]
        [Required]
        public string PictureUrl { get; set; }

        [Required]
        public string Game { get; set; }

        [Required]
        public string Class { get; set; }

        [Required]
        [MinLength(DescriptionMinLength)]
        public string Description { get; set; }
    }
}
