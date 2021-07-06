using System.ComponentModel.DataAnnotations;
using static GamingWiki.Web.Common.DataConstants;

namespace GamingWiki.Web.Models.Articles
{
    public class ArticleEditModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(DefaultMinLength)]
        [MaxLength(DefaultMaxLength)]
        public string Heading { get; set; }

        [Required]
        [MinLength(ContentMinLength)]
        public string Content { get; set; }

        [Url]
        [Required]
        public string PictureUrl { get; set; }
    }
}
