using System.ComponentModel.DataAnnotations;

namespace GamingWiki.Models
{
    public class Country
    {
        public string Id { get; set; }

        public string CountryCode { get; set; }

        public string Name { get; set; }

        public decimal Population { get; set; }
    }
}
