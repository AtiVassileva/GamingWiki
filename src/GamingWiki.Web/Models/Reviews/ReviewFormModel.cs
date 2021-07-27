using System.ComponentModel.DataAnnotations;
using GamingWiki.Services.Models.Games;
using GamingWiki.Web.Models.Games;
using static GamingWiki.Models.Common.DataConstants;

namespace GamingWiki.Web.Models.Reviews
{
    public class ReviewFormModel
    {
        [Range(MinimumRate, MaximumRate)]
        [Display(Name = "Price Rate ")]
        public int PriceRate { get; set; }

        [Range(MinimumRate, MaximumRate)]
        [Display(Name = "Levels Rate ")]
        public int LevelsRate { get; set; }

        [Range(MinimumRate, MaximumRate)]
        [Display(Name = "Graphics Rate ")]
        public int GraphicsRate { get; set; }

        [Range(MinimumRate, MaximumRate)]
        [Display(Name = "Difficulty Rate ")]
        public int DifficultyRate { get; set; }

        public string Description { get; set; }

        public GameServiceListingModel Game { get; set; }
    }
}
