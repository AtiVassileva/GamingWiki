using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamingWiki.Services.Models.Games;
using static GamingWiki.Models.Common.DataConstants;

namespace GamingWiki.Web.Models.Tricks
{
    public class TrickFormModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(HeadingMaxLength, 
            MinimumLength = DefaultMinLength)]
        public string Heading { get; set; }

        [Required]
        [StringLength(ContentMaxLength, 
            MinimumLength = ContentMinLength)]
        public string Content { get; set; }

        [Url]
        [Required]
        [Display(Name = "Picture URL")]
        public string PictureUrl { get; set; }

        [Display(Name = "Game")]
        public int GameId { get; set; }

        public IEnumerable<GameServiceSimpleModel> Games { get; set; }
    }
}
