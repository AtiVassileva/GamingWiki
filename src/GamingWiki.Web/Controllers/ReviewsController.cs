using GamingWiki.Services.Contracts;
using GamingWiki.Web.Infrastructure;
using GamingWiki.Web.Models.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService helper;

        public ReviewsController(IReviewService helper) 
            => this.helper = helper;

        [Authorize]
        public IActionResult All() => this.View(this.helper.All());

        [Authorize]
        public IActionResult Create(int gameId) 
            => this.View(new ReviewFormModel
            {
                Game = this.helper.GetGame(gameId)
            });

        [HttpPost]
        [Authorize]
        public IActionResult Create(ReviewFormModel model, int gameId)
        {
            if (!this.helper.GameExists(gameId))
            {
                this.ModelState.AddModelError(nameof(gameId), "Game does not exist.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            this.helper.Create(gameId, this.User.GetId(), model.PriceRate, model.LevelsRate, model.GraphicsRate,
                model.DifficultyRate, model.Description);

            return this.Redirect($"/Games/Details?gameId={gameId}");
        }

        [Authorize]
        public IActionResult Edit(int reviewId)
        {
            if (!this.helper.ReviewExists(reviewId))
            {
                return this.View("Error");
            }

            var authorId = this.helper.GetReviewAuthorId(reviewId);

            if (this.User.GetId() != authorId && !this.User.IsAdmin())
            {
                return this.Unauthorized();
            }

            return this.View(this.helper.GetReview(reviewId));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(ReviewFormModel model, int reviewId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Error");
            }

            this.helper.Edit(reviewId, model.PriceRate, model.LevelsRate, model.GraphicsRate, model.DifficultyRate, model.Description);

            return this.Redirect(nameof(this.All));
        }

        [Authorize]
        public IActionResult Delete(int reviewId)
        {
            if (!this.helper.ReviewExists(reviewId))
            {
                return this.View("Error");
            }

            var authorId = this.helper.GetReviewAuthorId(reviewId);

            if (this.User.GetId() != authorId && !this.User.IsAdmin())
            {
                return this.Unauthorized();
            }

            this.helper.Delete(reviewId);
            return this.Redirect(nameof(this.All));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Search(string searchCriteria) 
            => this.View(nameof(this.All), this.helper.Search(searchCriteria));
    }
}
