using System.Collections.Generic;
using GamingWiki.Services.Models.Games;
using GamingWiki.Services.Models.Reviews;

namespace GamingWiki.Services.Contracts
{
    public interface IReviewService
    {
        IEnumerable<ReviewDetailsServiceModel> All();

        GameServiceListingModel GetGame(int gameId);

        void Create(int gameId, string authorId, int priceRate, int levelsRate, int graphicsRate, int difficultyRate, string description);

        ReviewDetailsServiceModel GetReview(int reviewId);

        void Edit(int reviewId, int priceRate, int levelsRate, int graphicsRate, int difficultyRate, string description);

        void Delete(int reviewId);

        bool GameExists(int gameId);

        bool ReviewExists(int reviewId);

        string GetReviewAuthorId(int reviewId);

        IEnumerable<ReviewDetailsServiceModel> Search(string searchCriteria);
    }
}
