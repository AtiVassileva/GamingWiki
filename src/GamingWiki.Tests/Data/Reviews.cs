using System;
using System.Collections.Generic;
using System.Linq;
using GamingWiki.Models;
using GamingWiki.Services.Models.Games;
using GamingWiki.Web.Models.Reviews;
using static GamingWiki.Tests.Common.TestConstants;
using static GamingWiki.Tests.Data.Games;
using static GamingWiki.Tests.Data.Users;
using static GamingWiki.Models.Common.DataConstants;
namespace GamingWiki.Tests.Data
{
    public static class Reviews
    {
        public static IEnumerable<Review> FiveReviews
            => Enumerable.Range(0, 5).Select(_ => new Review());

        public static ReviewFormModel TestReviewValidFormModel
            => new()
            {
                PriceRate = GenerateRandomRate,
                DifficultyRate = GenerateRandomRate,
                GraphicsRate = GenerateRandomRate,
                LevelsRate = GenerateRandomRate,
                Description = DefaultDescription,
                Game =  new GameServiceListingModel
                {
                    Id = TestGame.Id,
                    Name = TestGame.Name,
                    PictureUrl = TestGame.PictureUrl
                }
            };
        
        public static ReviewFormModel TestReviewInvalidFormModel
            => new()
            {
                PriceRate = 0,
                DifficultyRate = 3,
                GraphicsRate = 1,
                LevelsRate = -2,
                Description = null,
                Game =  new GameServiceListingModel
                {
                    Id = TestGame.Id,
                    Name = TestGame.Name,
                    PictureUrl = TestGame.PictureUrl
                }
            };

        public static Review TestReview => new()
        {
            Id = DefaultId, 
            Game = TestGame,
            Author = TestUser
        };

        public static ReviewFormModel TestValidReviewEditModel
            => new()
            {

            };
        private static int GenerateRandomRate
            => new Random().Next(MinimumRate, MaximumRate);
    }
}
