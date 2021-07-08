using Microsoft.AspNetCore.Identity;

namespace GamingWiki.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; }

        public string AuthorId { get; set; }

        public IdentityUser Author { get; set; }

        public int PriceRate { get; set; }

        public int GraphicsRate { get; set; }

        public int LevelsRate { get; set; }

        public int DifficultyRate { get; set; }

        public string Description { get; set; }
    }
}
