using System;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Games;
using GamingWiki.Web.Models.Reviews;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public ReviewsController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public IActionResult All()
        {
            var reviewModels = this.dbContext
                .Reviews
                .Where(r => r.Description != null)
                .Select(r => new ReviewListingModel
                {
                    Id = r.Id,
                    Author = r.Author.UserName,
                    Game = new GameViewModel
                    {
                        Id = r.GameId,
                        Name = r.Game.Name,
                        PictureUrl = r.Game.PictureUrl
                    },
                    Description = r.Description,
                })
                .OrderByDescending(r => r.Id)
                .ToList();

            return this.View(reviewModels);
        }

        public IActionResult Create(int gameId)
        {
            var gameModel = this.dbContext
                .Games.Where(g => g.Id == gameId)
                .Select(g => new GameViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    PictureUrl = g.PictureUrl
                }).FirstOrDefault();

            return this.View(new ReviewFormModel
            {
                Game = gameModel
            });
        }

        [HttpPost]
        public IActionResult Create(ReviewFormModel model, int gameId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var reviewDto = new ReviewDtoModel
            {
                GameId = gameId,
                AuthorId = this.User.FindFirstValue(ClaimTypes.NameIdentifier),
                PriceRate = model.PriceRate,
                LevelsRate = model.LevelsRate,
                GraphicsRate = model.GraphicsRate,
                DifficultyRate = model.DifficultyRate,
                Description = model.Description,
            };

            var review = this.mapper.Map<Review>(reviewDto);

            this.dbContext.Reviews.Add(review);
            this.dbContext.SaveChanges();

            return this.Redirect($"/Games/Details?gameId={gameId}");
        }

        public IActionResult Edit(int reviewId)
        {
            var reviewModel = this.dbContext.Reviews
                .Where(r => r.Id == reviewId).Select(r => new ReviewListingModel
                {
                    Id = r.Id,
                    Author = r.Author.UserName,
                    Game = new GameViewModel
                    {
                        Id = r.GameId,
                        Name = r.Game.Name,
                        PictureUrl = r.Game.PictureUrl,
                    },
                    Description = r.Description
                }).FirstOrDefault();

            return this.View(reviewModel);
        }

        [HttpPost]
        public IActionResult Edit(ReviewFormModel model, int reviewId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Error", new ErrorViewModel
                {
                    RequestId = Guid.NewGuid().ToString()
                });
            }

            var review = this.dbContext
                .Reviews.First(r => r.Id == reviewId);

            review.PriceRate = model.PriceRate;
            review.LevelsRate = model.LevelsRate;
            review.GraphicsRate = model.GraphicsRate;
            review.DifficultyRate = model.DifficultyRate;
            review.Description = model.Description;

            this.dbContext.SaveChanges();

            return this.Redirect("/Reviews/All");
        }

        public IActionResult Delete(int reviewId)
        {
            var review = this.dbContext
                .Reviews.First(r => r.Id == reviewId);

            this.dbContext.Reviews.Remove(review);
            this.dbContext.SaveChanges();

            return this.Redirect("/Reviews/All");
        }

        [HttpPost]
        public IActionResult Search(string searchCriteria)
        {
            var reviewModel = this.dbContext.Reviews
                .Where(r => r.Game.Name.ToLower().Contains(searchCriteria.ToLower().Trim()) && r.Description != null)
                .Select(r => new ReviewListingModel
                {
                    Id = r.Id,
                    Author = r.Author.UserName,
                    Game = new GameViewModel
                    {
                        Id = r.GameId,
                        Name = r.Game.Name,
                        PictureUrl = r.Game.PictureUrl
                    },
                    Description = r.Description
                }).ToList();

            return View("All", reviewModel);
        }
    }
}
