namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Common.DataConstants;
    public class Area
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(AreaNameMaxLength)]
        public string Name { get; set; }
    }
}
