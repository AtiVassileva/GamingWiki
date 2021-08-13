using System.ComponentModel.DataAnnotations;
using static GamingWiki.Models.Common.DataConstants;

namespace GamingWiki.Web.Models.Discussions
{
    public class DiscussionFormModel
    {
        [Required]
        [StringLength(DefaultMaxLength, MinimumLength = DefaultMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; }

        [Url]
        [Required]
        [Display(Name = "Profile Picture URL")]
        public string PictureUrl { get; set; }

        [Range(MinDiscussionMembers, MaxDiscussionMembers)]
        [Display(Name = "Members Limit")]
        public int MembersLimit { get; set; }
    }
}
