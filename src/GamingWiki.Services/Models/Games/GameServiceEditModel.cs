using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamingWiki.Services.Models.Areas;
using GamingWiki.Services.Models.Platforms;
using static GamingWiki.Models.Common.DataConstants;

namespace GamingWiki.Services.Models.Games
{
    public class GameServiceEditModel
    {
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
        [StringLength(DescriptionMaxLength, 
            MinimumLength = DescriptionMinLength)]
        public string Description { get; set; }

        [Display(Name = "Area")]
        public int AreaId { get; set; }

        public string Area { get; set; }

        public IEnumerable<AreaServiceModel> Areas { get; set; }

        [Display(Name = "Supported Platforms")]
        public IEnumerable<int> SupportedPlatforms { get; set; }

        public IEnumerable<PlatformServiceModel> AllPlatforms { get; set; }
    }

}
