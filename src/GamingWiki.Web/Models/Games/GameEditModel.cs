using System.ComponentModel.DataAnnotations;

namespace GamingWiki.Web.Models.Games
{
    public class GameEditModel
    {
        public int Id { get; set; }

        [Required]
        [Url]
        public string PictureUrl { get; set; }

        [Required]
        [MinLength(10)]
        public string Description { get; set; }

        [Required]
        [MinLength(3)]
        [RegularExpression(@"[A-Za-z\s\.]+")]
        public string PlaceName { get; set; }

        [Required]
        public string PlaceType { get; set; }
    }

}
