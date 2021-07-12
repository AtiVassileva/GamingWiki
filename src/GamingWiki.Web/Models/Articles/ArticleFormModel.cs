using System.ComponentModel.DataAnnotations;
using static GamingWiki.Models.Common.DataConstants;

namespace GamingWiki.Web.Models.Articles
{
    public class ArticleFormModel
    {
        [Required]
        [StringLength(DefaultMaxLength, MinimumLength = DefaultMinLength)]
        public string Heading { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        [MinLength(ContentMinLength)]
        public string Content { get; set; }

        [Url]
        [Required]
        public string PictureUrl { get; set; }
    }
}
