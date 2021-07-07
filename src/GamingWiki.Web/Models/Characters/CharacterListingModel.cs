namespace GamingWiki.Web.Models.Characters
{
    public class CharacterListingModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Game { get; set; }

        public int GameId { get; set; }

        public string Class { get; set; }

        public string PictureUrl { get; set; }

        public string Description { get; set; }
    }
}
