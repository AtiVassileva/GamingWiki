using GamingWiki.Services.Models.Games;

namespace GamingWiki.Services.Models.Reviews
{
    public class ReviewDetailsServiceModel : ReviewServiceSimpleModel
    {
        public GameServiceListingModel Game { get; set; }
    }
}
