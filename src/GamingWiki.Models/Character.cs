using GamingWiki.Models.Common;
using Microsoft.AspNetCore.Identity;

namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;
    using static DataConstants;

    public class Character
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(CharacterNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string PictureUrl { get; set; }

        [Required]
        public string Description { get; set; }

        public int ClassId { get; set; }

        public Class Class { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; }

        public bool IsApproved { get; set; }

        [Required]
        public string ContributorId { get; set; }

        public IdentityUser Contributor { get; set; }
    }
}
