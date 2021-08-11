using Microsoft.AspNetCore.Identity;
using static GamingWiki.Models.Common.DataConstants;

namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class Discussion
    {
        public int Id { get; set; }

        [Required]
        public string CreatorId { get; set; }

        public IdentityUser Creator { get; set; }

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public ICollection<Message> Messages { get; set; }
        = new HashSet<Message>();

        public ICollection<UserDiscussion> UsersDiscussions { get; set; }
            = new HashSet<UserDiscussion>();
    }
}
