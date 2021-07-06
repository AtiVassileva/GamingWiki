using System;
using Microsoft.AspNetCore.Identity;

namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Reply
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime AddedOn { get; set; }

        public int CommentId { get; set; }

        public Comment Comment { get; set; }

        [Required]
        public string ReplierId { get; set; }

        public IdentityUser Replier { get; set; }
    }
}