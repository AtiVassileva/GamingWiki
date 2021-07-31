namespace GamingWiki.Services.Models.Tricks
{
    public class TrickServiceHomeModel
    {
        public int Id { get; set; }

        public string Heading { get; set; }

        public string Content { get; set; }

        public int GameId { get; set; }

        public string GameName { get; set; }
    }
}
