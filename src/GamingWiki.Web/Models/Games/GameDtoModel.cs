namespace GamingWiki.Web.Models.Games
{
    public class GameDtoModel
    {
        public string Name { get; set; }

        public string PictureUrl { get; set; }

        public string TrailerUrl { get; set; }

        public int GenreId { get; set; }

        public string Description { get; set; }

        public int AreaId { get; set; }
    }
}
