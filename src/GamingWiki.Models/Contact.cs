using Microsoft.AspNetCore.Identity;

namespace GamingWiki.Models
{
    public class Contact
    {
        public int Id { get; set; }

        public string AuthorId { get; set; }

        public IdentityUser Author { get; set; }

        public string Heading { get; set; }

        public string Content { get; set; }

        public string ReceiverId { get; set; }
    }
}
