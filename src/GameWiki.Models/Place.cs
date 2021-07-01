namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;
    using Enums;

    public class Place
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public PlaceType PlaceType { get; set; }
    }
}
