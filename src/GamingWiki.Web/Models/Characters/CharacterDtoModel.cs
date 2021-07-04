using GamingWiki.Models.Enums;

namespace GamingWiki.Web.Models.Characters
{
    public class CharacterDtoModel
    {
        public string Name { get; set; }

        public string PictureUrl { get; set; }

        public int GameId { get; set; }

        public CharacterClass Class { get; set; }

        public string Description { get; set; }
    }
}
