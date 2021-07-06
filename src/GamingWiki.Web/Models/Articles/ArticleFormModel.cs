using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using static GamingWiki.Web.Common.DataConstants;

namespace GamingWiki.Web.Models.Articles
{
    public class ArticleFormModel
    {
        [Required]
        [MinLength(DefaultMinLength)]
        [MaxLength(DefaultMaxLength)]
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
