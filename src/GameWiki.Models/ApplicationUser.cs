namespace GamingWiki.Models
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public int CountryId { get; set; }

        public Country Country { get; set; }

        public ICollection<Article> Articles { get; set; }
            = new HashSet<Article>();

        public ICollection<Comment> Comments { get; set; }
            = new HashSet<Comment>();

        public ICollection<Message> Messages { get; set; } 
            = new HashSet<Message>();

        public ICollection<Reply> Replies { get; set; }
        = new HashSet<Reply>();

        public ICollection<UserDiscussion> UserDiscussions { get; set; }
        = new HashSet<UserDiscussion>();

    }
}
