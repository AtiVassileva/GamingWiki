using System.Collections.Generic;
using System.Linq;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Games;
using GamingWiki.Services.Models.Reviews;

namespace GamingWiki.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext dbContext;

        public ReviewService(ApplicationDbContext dbContext)
            => this.dbContext = dbContext;

        public IEnumerable<ReviewDetailsServiceModel> All()
            => this.GetReviews(this.dbContext.Reviews
                .Where(r => r.Description != null));

        public GameServiceListingModel GetGame(int gameId)
        => this.dbContext.Games
            .Where(g => g.Id == gameId)
            .Select(g => new GameServiceListingModel
            {
                Id = g.Id,
                Name = g.Name,
                PictureUrl = g.PictureUrl
            }).FirstOrDefault();

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

        public void Edit(int reviewId, int priceRate, int levelsRate, int graphicsRate, int difficultyRate, string description)
        {
            var review = this.dbContext
                .Reviews.FirstOrDefault(r => r.Id == reviewId);

            if (review == null)
            {
                return;
            }

            review.PriceRate = priceRate;
            review.LevelsRate = levelsRate;
            review.GraphicsRate = graphicsRate;
            review.DifficultyRate = difficultyRate;
            review.Description = description;

            this.dbContext.SaveChanges();
        }

        public void Delete(int reviewId)
        {
            var review = this.dbContext
                .Reviews.FirstOrDefault(r => r.Id == reviewId);

            if (review == null)
            {
                return;
            }

            this.dbContext.Reviews.Remove(review);
            this.dbContext.SaveChanges();
        }

        public bool GameExists(int gameId)
            => this.dbContext.Games.Any(g => g.Id == gameId);

        public bool ReviewExists(int reviewId)
            => this.dbContext.Reviews.Any(r => r.Id == reviewId);

        public string GetReviewAuthorId(int reviewId)
            => this.dbContext.Reviews
                .First(r => r.Id == reviewId).AuthorId;

        public IEnumerable<ReviewDetailsServiceModel> Search(string searchCriteria)
            => this.GetReviews(this.dbContext.Reviews
                .Where(r => r.Game.Name.ToLower()
                                .Contains(searchCriteria.ToLower().Trim())
                            && r.Description != null));
            

        private IEnumerable<ReviewDetailsServiceModel> GetReviews(IQueryable<Review> reviewsQuery)
        => reviewsQuery
            .Select(r => new ReviewDetailsServiceModel
            {
                Id = r.Id,
                Author = r.Author.UserName,
                Game = new GameServiceListingModel
                {
                    Id = r.GameId,
                    Name = r.Game.Name,
                    PictureUrl = r.Game.PictureUrl
                },
                Description = r.Description
            }).ToList();

    }
}
