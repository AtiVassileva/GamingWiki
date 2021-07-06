using System.ComponentModel.DataAnnotations;
using static GamingWiki.Web.Common.DataConstants;

namespace GamingWiki.Web.Models.Replies
{
    public class ReplyFormModel
    {
        [Required]
        [MinLength(ContentMinLength)]
        public string ReplyContent { get; set; }
    }
}
