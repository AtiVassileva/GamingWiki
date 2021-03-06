using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using static GamingWiki.Models.Common.DataConstants;
namespace GamingWiki.Models
{
    public class Trick
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(HeadingMaxLength)]
        public string Heading { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public string PictureUrl { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public IdentityUser Author { get; set; }
    }
}
