using System.ComponentModel.DataAnnotations;
using GamingWiki.Models.Common;

namespace GamingWiki.Models
{
    using static DataConstants;

    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; }
    }
}
