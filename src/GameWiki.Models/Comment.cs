using System.Collections.Generic;

namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public int ArticleId { get; set; }

        public Article Article { get; set; }

        [Required]
        public string CommenterId { get; set; }

        public ApplicationUser Commenter { get; set; }

        public ICollection<Reply> Replies { get; set; }
        = new HashSet<Reply>();
    }
}
