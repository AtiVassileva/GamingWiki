namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    
    public class Country
    {
        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        public string CountryCode { get; set; }

        [Required]
        public string Name { get; set; }

        public long Population { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
        = new HashSet<ApplicationUser>();
    }
}
