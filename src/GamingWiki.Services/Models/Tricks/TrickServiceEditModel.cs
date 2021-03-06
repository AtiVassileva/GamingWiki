using System.ComponentModel.DataAnnotations;
using static GamingWiki.Models.Common.DataConstants;

namespace GamingWiki.Services.Models.Tricks
{
    public class TrickServiceEditModel
    {
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
    }
}
