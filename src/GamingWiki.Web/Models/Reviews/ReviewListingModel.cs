using GamingWiki.Web.Models.Games;

namespace GamingWiki.Web.Models.Reviews
{
    public class ReviewListingModel
    {
        public int Id { get; set; }

        public GameSimpleModel Game { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }
    }
}
