namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Reply
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public int CommentId { get; set; }

        public Comment Comment { get; set; }

        [Required]
        public string ReplierId { get; set; }

        public ApplicationUser Replier { get; set; }
    }
}