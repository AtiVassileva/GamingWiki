using System.ComponentModel.DataAnnotations;
using static GamingWiki.Models.Common.DataConstants;
namespace GamingWiki.Models
{
    public class Platform
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(PlatformNameMaxLength)]
        public string Name { get; set; }
    }
}
