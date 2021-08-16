using System.Collections.Generic;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Reviews;
using GamingWiki.Web.Infrastructure;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GamingWiki.Web.Common.ExceptionMessages;
using static GamingWiki.Web.Common.AlertMessages;
using static GamingWiki.Web.Common.WebConstants;

namespace GamingWiki.Web.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private const int ReviewsPerPage = 3;

        private readonly IReviewService reviewService;

        public ReviewsController(IReviewService helper) 
            => this.reviewService = helper;
        
        public IActionResult All(int pageIndex = 1) 
            => this.View(new ReviewFullModel
            {
                Reviews = PaginatedList<ReviewDetailsServiceModel>
                    .Create(this.reviewService.All(),
                        pageIndex, ReviewsPerPage),
                Tokens = new KeyValuePair<object, object>("All", null)
            });
        
        public IActionResult Create(int gameId) 
            => this.View(new ReviewFormModel
            {
                Game = this.reviewService.GetGame(gameId)
            });

        [HttpPost]
        public IActionResult Create(ReviewFormModel model, int gameId)
        {
            if (!this.reviewService.GameExists(gameId))
            {
                this.ModelState.AddModelError(nameof(gameId), NonExistingGameExceptionMessage);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            this.reviewService.Create(gameId, this.User.GetId(), model.PriceRate, model.LevelsRate, model.GraphicsRate,
                model.DifficultyRate, model.Description);

            TempData[GlobalMessageKey] = SuccessfullyAddedReviewMessage;

            return RedirectToAction(nameof(GamesController.Details), "Games", new { gameId });
        }
        
        public IActionResult Edit(int reviewId)
        {
            if (!this.reviewService.ReviewExists(reviewId))
            {
                return this.View("Error", CreateError(NonExistingReviewExceptionMessage));
            }

            var authorId = this.reviewService.GetReviewAuthorId(reviewId);

            if (this.User.GetId() != authorId && !this.User.IsAdmin())
            {
                return this.Unauthorized();
            }

            var reviewDetails = this.reviewService.Details(reviewId);

            return this.View(reviewDetails);
        }

        [HttpPost]
        public IActionResult Edit(ReviewFormModel model, int reviewId)
        {
            if (!this.reviewService.ReviewExists(reviewId))
            {
                return this.View("Error", CreateError(NonExistingReviewExceptionMessage));
            }

            if (!this.ModelState.IsValid)
            {
                var reviewDetails = this.reviewService.Details(reviewId);
                return this.View(reviewDetails);
            }

            var edited = this.reviewService.Edit(reviewId, model.PriceRate, model.LevelsRate, model.GraphicsRate, model.DifficultyRate, model.Description);

            if (!edited)
            {
                return this.BadRequest();
            }

            TempData[GlobalMessageKey] = SuccessfullyEditedReviewMessage;

            return RedirectToAction(nameof(GamesController.Details), "Games", new { gameId = model.Game.Id});
        }
        
        public IActionResult Delete(int reviewId)
        {
            if (!this.reviewService.ReviewExists(reviewId))
            {
                return this.View("Error", CreateError(NonExistingReviewExceptionMessage));
            }

            var authorId = this.reviewService.GetReviewAuthorId(reviewId);

            if (!this.User.IsAdmin() && this.User.GetId() != authorId)
            {
                return this.Unauthorized();
            }

            var deleted = this.reviewService.Delete(reviewId);

            if (!deleted)
            {
                return this.BadRequest();
            }

            TempData[GlobalMessageKey] = DeletedReviewMessage;

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Search(string parameter, int pageIndex = 1, string name = null) 
            => this.View(nameof(this.All), new ReviewFullModel
            {
                Reviews = PaginatedList<ReviewDetailsServiceModel>
                    .Create(this.reviewService.Search(parameter),
                        pageIndex, ReviewsPerPage),
                Tokens = new KeyValuePair<object, object>("Search", parameter)
            });

        [HttpPost]
        public IActionResult Search(string searchCriteria, 
            int pageIndex = 1) 
            => this.View(nameof(this.All), new ReviewFullModel
            {
                Reviews = PaginatedList<ReviewDetailsServiceModel>
                    .Create(this.reviewService.Search(searchCriteria),
                        pageIndex, ReviewsPerPage),
                Tokens = new KeyValuePair<object, object>("Search", searchCriteria)
            });

        public IActionResult Mine(int pageIndex = 1)
            => this.View(nameof(this.All), new ReviewFullModel
            {
                Reviews = PaginatedList<ReviewDetailsServiceModel>
                    .Create(this.reviewService.GetReviewsByUser
                            (this.User.GetId()),
                        pageIndex, ReviewsPerPage),
                Tokens = new KeyValuePair<object, object>("Mine", null)
            });
    }
}
