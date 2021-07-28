using GamingWiki.Services.Models.Games;

namespace GamingWiki.Services.Models.Reviews
{
    public class ReviewDetailsServiceModel
    {
        public int Id { get; set; }

        public GameServiceListingModel Game { get; set; }

        public string Author { get; set; }

        public string AuthorId { get; set; }

        public string Description { get; set; }
    }
}
