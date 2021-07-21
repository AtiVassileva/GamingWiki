using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamingWiki.Web.Models.Classes;
using GamingWiki.Web.Models.Games;
using static GamingWiki.Models.Common.DataConstants;

namespace GamingWiki.Web.Models.Characters
{
    public class CharacterFormModel
    {
        [Required]
        [StringLength(DefaultMaxLength, MinimumLength = DefaultMinLength)]
        public string Name { get; set; }

        [Url]
        [Required]
        [Display(Name = "Picture URL")]
        public string PictureUrl { get; set; }

        [Display(Name = "Game")]
        public int GameId { get; set; }

        [Display(Name = "Class")]
        public int ClassId { get; set; }

        [Required]
        [MinLength(DescriptionMinLength)]
        public string Description { get; set; }

        public IEnumerable<GameSimpleModel> Games { get; set; }

        public IEnumerable<ClassViewModel> Classes { get; set; }
    }
}
