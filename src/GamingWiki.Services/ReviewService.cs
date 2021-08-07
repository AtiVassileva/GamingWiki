using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Games;
using GamingWiki.Services.Models.Reviews;
using Microsoft.EntityFrameworkCore;

namespace GamingWiki.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IConfigurationProvider configuration;

        public ReviewService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.configuration = mapper.ConfigurationProvider;
        }

        public IQueryable<ReviewDetailsServiceModel> All()
            => this.GetReviews(this.dbContext.Reviews
                .Where(r => r.Description != null));

        public GameServiceListingModel GetGame(int gameId)
        => this.dbContext.Games
            .Where(g => g.Id == gameId)
            .ProjectTo<GameServiceListingModel>(this.configuration)
            .FirstOrDefault();

        public void Create(int gameId, string authorId, int priceRate, int levelsRate, int graphicsRate, int difficultyRate, string description)
        {
            var review = new Review
            {
                GameId = gameId,
                AuthorId = authorId,
                PriceRate = priceRate,
                LevelsRate = levelsRate,
                GraphicsRate = graphicsRate,
                DifficultyRate = difficultyRate,
                Description = description,
            };

            this.dbContext.Reviews.Add(review);
            this.dbContext.SaveChanges();
        }

        public ReviewDetailsServiceModel GetReview(int reviewId)
            => this.GetReviews(this.dbContext.Reviews
                .Where(r => r.Id == reviewId))
                .FirstOrDefault();

        public ReviewDetailsServiceModel Details(int reviewId)
        {
            var dbModel = this.FindReview(reviewId);

            var detailsModel = this.mapper
                .Map<ReviewDetailsServiceModel>(dbModel);

            detailsModel.Game = this.mapper
                .Map<GameServiceListingModel>(dbModel.Game);

            return detailsModel;
        }

        public bool Edit(int reviewId, int priceRate, int levelsRate, int graphicsRate, int difficultyRate, string description)
        {
            if (!this.ReviewExists(reviewId))
            {
                return false;
            }

            var review = this.FindReview(reviewId);

            review.PriceRate = priceRate;
            review.LevelsRate = levelsRate;
            review.GraphicsRate = graphicsRate;
            review.DifficultyRate = difficultyRate;
            review.Description = description;

            this.dbContext.SaveChanges();

            return true;
        }

        public bool Delete(int reviewId)
        {
            if (!this.ReviewExists(reviewId))
            {
                return false;
            }

            var review = this.FindReview(reviewId);

            this.dbContext.Reviews.Remove(review);
            this.dbContext.SaveChanges();

            return true;
        }

        public bool GameExists(int gameId)
            => this.dbContext.Games.Any(g => g.Id == gameId);

        public bool ReviewExists(int reviewId)
            => this.dbContext.Reviews.Any(r => r.Id == reviewId);

        public string GetReviewAuthorId(int reviewId)
            => this.dbContext.Reviews
                .First(r => r.Id == reviewId).AuthorId;

        public IQueryable<ReviewDetailsServiceModel> Search(string searchCriteria)
            => this.GetReviews(this.dbContext.Reviews
                .Where(r => r.Game.Name.ToLower()
                                .Contains(searchCriteria
                                    .ToLower().Trim())
                            && r.Description != null));

        public IQueryable<ReviewDetailsServiceModel> GetReviewsByUser(string userId)
            => this.GetReviews(this.dbContext.Reviews
                .Where(r => r.AuthorId == userId));


        private IQueryable<ReviewDetailsServiceModel> GetReviews(IQueryable reviewsQuery)
        {
            var reviewsModels = reviewsQuery
                .ProjectTo<ReviewDetailsServiceModel>(this.configuration);

            foreach (var model in reviewsModels)
            {
                model.Game = this.mapper
                    .Map<GameServiceListingModel>(model.Game);
            }

            return reviewsModels
                .OrderByDescending(r => r.Id);
        }

        private Review FindReview(int reviewId)
            => this.dbContext.Reviews
                .Include(r => r.Author)
                .Include(r => r.Game)
                .First(r => r.Id == reviewId);

    }
}
