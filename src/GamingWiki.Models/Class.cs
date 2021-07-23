using System.ComponentModel.DataAnnotations;

namespace GamingWiki.Models
{
    using static Common.DataConstants;
    public class Class
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(CharacterClassMaxLength)]
        public string Name { get; set; }
    }
}
