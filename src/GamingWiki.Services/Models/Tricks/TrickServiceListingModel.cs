namespace GamingWiki.Services.Models.Tricks
{
    public class TrickServiceListingModel
    {
        public int Id { get; set; }

        public string Heading { get; set; }

        public string Content { get; set; }

        public string PictureUrl { get; set; }

        public string Game { get; set; }

        public int GameId { get; set; }

        public string Author { get; set; }

        public string AuthorId { get; set; }
    }
}
