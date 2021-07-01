using System.Collections.Generic;

namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;
    using Enums;

    public class Article
    {
        public int Id { get; set; }

        [Required]
        public string Heading { get; set; }

        [Required]
        public string Content { get; set; }

        public Category Category { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }

        public ICollection<Comment> Comments { get; set; }
        = new HashSet<Comment>();

    }
}