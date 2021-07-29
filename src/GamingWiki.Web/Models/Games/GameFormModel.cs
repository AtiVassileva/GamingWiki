using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamingWiki.Services.Models.Areas;
using GamingWiki.Services.Models.Genres;
using GamingWiki.Services.Models.Platforms;
using static GamingWiki.Models.Common.DataConstants;

namespace GamingWiki.Web.Models.Games
{
    public class GameFormModel
    {
        [Required]
        [StringLength(DefaultMaxLength, MinimumLength = DefaultMinLength, ErrorMessage = "Name should be between {2} and {1} symbols.")]
        public string Name { get; set; }

        [Url]
        [Required]
        [Display(Name = "Picture URL")]
        public string PictureUrl { get; set; }

        [Url]
        [Required]
        [Display(Name = "Trailer URL")]
        public string TrailerUrl { get; set; }

        [Required]
        [MinLength(DescriptionMinLength)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Creators (separated by ', ')")]
        public string CreatorsNames { get; set; }

        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        [Display(Name = "Area")]
        public int AreaId { get; set; }

        [Display(Name = "Platforms")]
        public IEnumerable<int> SelectedPlatforms { get; set; }

        public IEnumerable<GenreServiceModel> Genres { get; set; }

        public IEnumerable<AreaServiceModel> Areas { get; set; }

        public IEnumerable<PlatformServiceModel> Platforms { get; set; }
    }
}
