using System.Collections.Generic;
using AutoMapper;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Tricks;
using GamingWiki.Web.Infrastructure;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Tricks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GamingWiki.Web.Common.ExceptionMessages;
using static GamingWiki.Web.Common.AlertMessages;
using static GamingWiki.Web.Common.WebConstants;

namespace GamingWiki.Web.Controllers
{
    [Authorize]
    public class TricksController : Controller
    {
        private const int TricksPerPage = 3;

        private readonly ITrickService trickService;
        private readonly IMapper mapper;

        public TricksController(ITrickService helper, IMapper mapper)
        {
            this.trickService = helper;
            this.mapper = mapper;
        }

        public IActionResult All(int pageIndex = 1) 
            => this.View(new TrickFullModel
            {
                Tricks = PaginatedList<TrickServiceListingModel>
                    .Create(this.trickService.All(),
                        pageIndex, TricksPerPage),
                Tokens = new KeyValuePair<object, object>("All", null)
            });
        
        public IActionResult Create()
            => this.View(new TrickFormModel
            {
                Games = this.trickService.GetGames()
            });

        [HttpPost]
        public IActionResult Create(TrickFormModel model)
        {
            if (!this.trickService.GameExists(model.GameId))
            {
                this.ModelState.AddModelError(nameof(model.GameId), NonExistingGameExceptionMessage);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var authorId = this.User.GetId();

            this.trickService.Create(model.Heading, model.Content, authorId, model.PictureUrl, model.GameId);

            TempData[GlobalMessageKey] = SuccessfullyAddedTrickMessage;

            return this.RedirectToAction(nameof(this.All));
        }
        
        public IActionResult Edit(int trickId)
        {
            if (!this.trickService.TrickExists(trickId))
            {
                return this.View("Error", CreateError(NonExistingTrickExceptionMessage));
            }

            var detailsModel = this.trickService.Details(trickId);

            if (!this.User.IsAdmin() && this.User.GetId() != detailsModel.AuthorId)
            {
                return this.Unauthorized();
            }

            return this.View(this.mapper
                .Map<TrickServiceEditModel>(detailsModel));
        }

        [HttpPost]
        public IActionResult Edit(TrickServiceEditModel model, int trickId)
        {
            if (!this.ModelState.IsValid)
            {
                var detailsModel = this.trickService.Details(trickId);

                model = this.mapper
                    .Map<TrickServiceEditModel>(detailsModel);
                
                return this.View(model);
            }

            var edited = this.trickService.Edit(trickId, model.Heading, model.Content, model.PictureUrl);

            if (!edited)
            {
                return this.BadRequest();
            }

            TempData[GlobalMessageKey] = SuccessfullyEditedTrickMessage;

            return this.RedirectToAction(nameof(this.All));
        }
        
        public IActionResult Delete(int trickId)
        {
            if (!this.trickService.TrickExists(trickId))
            {
                return this.View("Error", CreateError(NonExistingTrickExceptionMessage));
            }

            var trickAuthorId = this.trickService.GetTrickAuthorId(trickId);

            if (!this.User.IsAdmin() && this.User.GetId() != trickAuthorId)
            {
                return this.Unauthorized();
            }

            var deleted = this.trickService.Delete(trickId);

            if (!deleted)
            {
                return this.BadRequest();
            }

            TempData[GlobalMessageKey] = DeletedTrickMessage;

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Search(string parameter,
            int pageIndex = 1, string name = null)
            => this.View(nameof(this.All), new TrickFullModel
            {
                Tricks = PaginatedList<TrickServiceListingModel>
                    .Create(this.trickService.Search(parameter),
                        pageIndex, TricksPerPage),
                Tokens = new KeyValuePair<object, object>("Search", parameter)
            });

        [HttpPost]
        public IActionResult Search(string searchCriteria, 
            int pageIndex = 1) 
            => this.View(nameof(this.All), new TrickFullModel
            {
                Tricks = PaginatedList<TrickServiceListingModel>
                    .Create(this.trickService.Search(searchCriteria),
                        pageIndex, TricksPerPage),
                Tokens = new KeyValuePair<object, object>("Search", searchCriteria)
            });

        public IActionResult Mine(int pageIndex = 1)
            => this.View(nameof(this.All), new TrickFullModel
            {
                Tricks = PaginatedList<TrickServiceListingModel>
                    .Create(this.trickService
                            .GetTricksByUser(this.User.GetId()),
                        pageIndex, TricksPerPage),
                Tokens = new KeyValuePair<object, object>("Mine", null)
            });
    }
}
