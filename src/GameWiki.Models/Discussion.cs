using Microsoft.AspNetCore.Identity;

namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using Enums;

    public class Discussion
    {
        public int Id { get; set; }

        [Required]
        public string CreatorId { get; set; }

        public IdentityUser Creator { get; set; }

        public Category Category { get; set; }

        public ICollection<Message> Messages { get; set; }
        = new HashSet<Message>();

        public ICollection<UserDiscussion> UsersDiscussions { get; set; }
            = new HashSet<UserDiscussion>();
    }
}
