using System;
using Microsoft.AspNetCore.Identity;

namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public int DiscussionId { get; set; }

        public Discussion Discussion { get; set; }

        [Required]
        public string SenderId { get; set; }

        public IdentityUser Sender { get; set; }

        public DateTime SentOn { get; set; }
    }
}
