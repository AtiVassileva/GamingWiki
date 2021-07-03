using System.ComponentModel.DataAnnotations;
using GamingWiki.Models.Enums;

namespace GamingWiki.Web.Models.Games
{
    public class GameDtoModel
    {
        public string Name { get; set; }

        public string PictureUrl { get; set; }

        public string Description { get; set; }

        public int PlaceId { get; set; }
    }
}
