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
        [MinLength(DefaultMinLength)]
        [MaxLength(HeadingMaxLength)]
        public string Heading { get; set; }

        [Required]
        [MinLength(ContentMinLength)]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [Url]
        [Required]
        public string PictureUrl { get; set; }

        public int GameId { get; set; }

        public IEnumerable<GameServiceSimpleModel> Games { get; set; }
    }
}
