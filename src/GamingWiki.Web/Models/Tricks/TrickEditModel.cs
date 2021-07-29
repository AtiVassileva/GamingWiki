using System.ComponentModel.DataAnnotations;
using static GamingWiki.Models.Common.DataConstants;

namespace GamingWiki.Web.Models.Tricks
{
    public class TrickEditModel
    {
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
        [Display(Name = "Picture URL")]
        public string PictureUrl { get; set; }
    }
}
