using System;
using System.Collections.Generic;
using GamingWiki.Models.Common;
using Microsoft.AspNetCore.Identity;

namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;
    using Enums;
    using static DataConstants;

    public class Article
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(HeadingMaxLength)]
        public string Heading { get; set; }

        [Required]
        public string Content { get; set; }

        public int CategoryId { get; set; }  
        
        public Category Category { get; set; }

        public DateTime PublishedOn { get; set; }

        [Required]
        public string PictureUrl { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public IdentityUser Author { get; set; }

        public ICollection<Comment> Comments { get; set; }
        = new HashSet<Comment>();
    }
}