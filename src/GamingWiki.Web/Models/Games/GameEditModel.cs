using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamingWiki.Web.Models.Areas;
using static GamingWiki.Models.Common.DataConstants;

namespace GamingWiki.Web.Models.Games
{
    public class GameEditModel
    {
        public int Id { get; set; }

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

        [Display(Name = "Area")]
        public int AreaId { get; set; }

        public IEnumerable<AreaViewModel> Areas { get; set; }
    }

}
