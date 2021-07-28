using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Common.DataConstants;
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        public DateTime AddedOn { get; set; }

        public int ArticleId { get; set; }

        public Article Article { get; set; }

        [Required]
        public string CommenterId { get; set; }

        public IdentityUser Commenter { get; set; }

        public ICollection<Reply> Replies { get; set; }
        = new HashSet<Reply>();
    }
}
