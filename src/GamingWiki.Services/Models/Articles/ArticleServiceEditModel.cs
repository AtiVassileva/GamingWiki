using System.ComponentModel.DataAnnotations;
using static GamingWiki.Models.Common.DataConstants;

namespace GamingWiki.Services.Models.Articles
{
    public class ArticleServiceEditModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(DefaultMaxLength, MinimumLength = DefaultMinLength)]
        public string Heading { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = ContentMinLength)]
        public string Content { get; set; }

        [Url]
        [Required]
        [Display(Name = "Picture URL")]
        public string PictureUrl { get; set; }
    }
}
