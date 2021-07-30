using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamingWiki.Services.Models.Classes;
using GamingWiki.Services.Models.Games;
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
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; }

        public IEnumerable<GameServiceSimpleModel> Games { get; set; }

        public IEnumerable<ClassSimpleServiceModel> Classes { get; set; }
    }
}
