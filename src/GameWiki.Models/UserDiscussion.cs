﻿namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserDiscussion
    {
        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int DiscussionId { get; set; }

        public Discussion Discussion { get; set; }
    }
}
