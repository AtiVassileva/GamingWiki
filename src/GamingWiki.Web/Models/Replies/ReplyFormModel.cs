using System.ComponentModel.DataAnnotations;

namespace GamingWiki.Web.Models.Replies
{
    public class ReplyFormModel
    {
        [Required]
        public string ReplyContent { get; set; }
    }
}
