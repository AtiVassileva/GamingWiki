using System.ComponentModel.DataAnnotations;
using static GamingWiki.Models.Common.DataConstants;

namespace GamingWiki.Web.Models.Comments
{
    public class CommentFormModel
    {
        [Required]
        [StringLength(CommentContentMaxLength, 
            MinimumLength = CommentContentMinLength)]
        public string Content { get; set; }
    }
}
