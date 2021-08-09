using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamingWiki.Services.Models.Categories;
using Newtonsoft.Json.Serialization;
using static GamingWiki.Models.Common.DataConstants;

namespace GamingWiki.Web.Models.Articles
{
    public class ArticleFormModel
    {
        [Required]
        [StringLength(DefaultMaxLength, MinimumLength = DefaultMinLength)]

        public string Heading { get; set; }
        
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        public string Content { get; set; }

        [Url]
        [Required]
        [Display(Name = "Picture URL")]
        public string PictureUrl { get; set; }

        public IEnumerable<CategoryServiceModel> Categories { get; set; }
    }
}
