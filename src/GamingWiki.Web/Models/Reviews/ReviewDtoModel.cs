namespace GamingWiki.Web.Models.Reviews
{
    public class ReviewDtoModel
    {
        public int GameId { get; set; }

        public string AuthorId { get; set; }

        public int PriceRate { get; set; }

        public int LevelsRate { get; set; }

        public int GraphicsRate { get; set; }

        public int DifficultyRate { get; set; }


        public string Description { get; set; }
    }
}
