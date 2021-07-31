using GamingWiki.Services.Contracts;
using GamingWiki.Web.Infrastructure;
using GamingWiki.Web.Models.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GamingWiki.Web.Common.ExceptionMessages;
using static GamingWiki.Web.Common.WebConstants;

namespace GamingWiki.Web.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly IReviewService helper;

        public ReviewsController(IReviewService helper) 
            => this.helper = helper;
        
        public IActionResult All() => this.View(this.helper.All());
        
        public IActionResult Create(int gameId) 
            => this.View(new ReviewFormModel
            {
                Game = this.helper.GetGame(gameId)
            });

        [HttpPost]
        public IActionResult Create(ReviewFormModel model, int gameId)
        {
            if (!this.helper.GameExists(gameId))
            {
                this.ModelState.AddModelError(nameof(gameId), NonExistingGameExceptionMessage);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            this.helper.Create(gameId, this.User.GetId(), model.PriceRate, model.LevelsRate, model.GraphicsRate,
                model.DifficultyRate, model.Description);

            return this.Redirect($"/Games/Details?gameId={gameId}");
        }
        
        public IActionResult Edit(int reviewId)
        {
            if (!this.helper.ReviewExists(reviewId))
            {
                return this.View("Error", CreateError(NonExistingReviewExceptionMessage));
            }

            var authorId = this.helper.GetReviewAuthorId(reviewId);

            if (this.User.GetId() != authorId && !this.User.IsAdmin())
            {
                return this.Unauthorized();
            }

            return this.View(this.helper.Details(reviewId));
        }

        [HttpPost]
        public IActionResult Edit(ReviewFormModel model, int reviewId)
        {
            if (!this.helper.ReviewExists(reviewId))
            {
                return this.View("Error", CreateError(NonExistingReviewExceptionMessage));
            }

            if (!this.ModelState.IsValid)
            {
                var reviewDetails = this.helper.Details(reviewId);
                return this.View(reviewDetails);
            }

            this.helper.Edit(reviewId, model.PriceRate, model.LevelsRate, model.GraphicsRate, model.DifficultyRate, model.Description);

            return this.Redirect(nameof(this.All));
        }
        
        public IActionResult Delete(int reviewId)
        {
            if (!this.helper.ReviewExists(reviewId))
            {
                return this.View("Error", CreateError(NonExistingReviewExceptionMessage));
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
        public IActionResult Search(string searchCriteria) 
            => this.View(nameof(this.All), this.helper.Search(searchCriteria));
    }
}
