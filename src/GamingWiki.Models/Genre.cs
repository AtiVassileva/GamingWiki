using System.ComponentModel.DataAnnotations;

namespace GamingWiki.Models
{
    using static Common.DataConstants;
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(GenreNameMaxLength)]
        public string Name { get; set; }
    }
}
