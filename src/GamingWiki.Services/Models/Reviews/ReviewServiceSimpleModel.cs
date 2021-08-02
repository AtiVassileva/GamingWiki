namespace GamingWiki.Services.Models.Reviews
{
    public class ReviewServiceSimpleModel
    {
        public int Id { get; set; }

        public string Author { get; set; }

        public string AuthorId { get; set; }

        public string Description { get; set; }

        public int PriceRate { get; set; }

        public int LevelsRate { get; set; }

        public int GraphicsRate { get; set; }

        public int DifficultyRate { get; set; }
    }
}
