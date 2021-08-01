using System.Collections.Generic;
using System.Linq;
using GamingWiki.Services.Models.Games;
using GamingWiki.Services.Models.Reviews;

namespace GamingWiki.Services.Contracts
{
    public interface IReviewService
    {
        IQueryable<ReviewDetailsServiceModel> All();

        GameServiceListingModel GetGame(int gameId);

        void Create(int gameId, string authorId, int priceRate, int levelsRate, int graphicsRate, int difficultyRate, string description);

        ReviewDetailsServiceModel GetReview(int reviewId);

        ReviewDetailsServiceModel Details(int reviewId);

        void Edit(int reviewId, int priceRate, int levelsRate, int graphicsRate, int difficultyRate, string description);

        void Delete(int reviewId);

        bool GameExists(int gameId);

        bool ReviewExists(int reviewId);

        string GetReviewAuthorId(int reviewId);

        IQueryable<ReviewDetailsServiceModel> Search(string searchCriteria);

        IQueryable<ReviewDetailsServiceModel> GetReviewsByUser(string userId);
    }
}
