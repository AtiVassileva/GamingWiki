using Microsoft.AspNetCore.Identity;

namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    
    public class Country
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string CountryCode { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public long Population { get; set; }

        public ICollection<IdentityUser> Users { get; set; }
        = new HashSet<IdentityUser>();
    }
}
