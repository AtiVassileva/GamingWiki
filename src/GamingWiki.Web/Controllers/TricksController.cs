using GamingWiki.Services.Contracts;
using GamingWiki.Web.Infrastructure;
using GamingWiki.Web.Models.Tricks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    public class TricksController : Controller
    {
        private readonly ITrickService helper;

        public TricksController(ITrickService helper) 
            => this.helper = helper;

        [Authorize]
        public IActionResult All()
        {
            var col = this.helper.All();
            return this.View(col);
        }

        [Authorize]
        public IActionResult Create()
            => this.View(new TrickFormModel
            {
                Games = this.helper.GetGames()
            });

        [HttpPost]
        [Authorize]
        public IActionResult Create(TrickFormModel model)
        {
            if (!this.helper.GameExists(model.GameId))
            {
                this.ModelState.AddModelError(nameof(model.GameId), "GameName does not exist.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var authorId = this.User.GetId();

            this.helper.Create(model.Heading, model.Content, authorId, model.PictureUrl, model.GameId);

            return this.RedirectToAction(nameof(this.All));
        }

        [Authorize]
        public IActionResult Edit(int trickId)
        {
            if (!this.helper.TrickExists(trickId))
            {
                return this.View("Error");
            }

            var trick = this.helper.Details(trickId);

            if (!this.User.IsAdmin() && this.User.GetId() != trick.AuthorId)
            {
                return this.Unauthorized();
            }

            return this.View(new TrickEditModel
            {
                Heading = trick.Heading,
                Content = trick.Content,
                PictureUrl = trick.PictureUrl
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(TrickEditModel model, int trickId)
        {
            if (!this.ModelState.IsValid)
            {
                var trick = this.helper.Details(trickId);

                model = new TrickEditModel
                {
                    Heading = trick.Heading,
                    Content = trick.Content,
                    PictureUrl = trick.PictureUrl
                };

                return this.View(model);
            }

            this.helper.Edit(trickId, model.Heading, model.Content, model.PictureUrl);

            return this.RedirectToAction(nameof(this.All));
        }

        [Authorize]
        public IActionResult Delete(int trickId)
        {
            if (!this.helper.TrickExists(trickId))
            {
                return this.View("Error");
            }

            var trickAuthorId = this.helper.GetTrickAuthorId(trickId);

            if (!this.User.IsAdmin() && this.User.GetId() != trickAuthorId)
            {
                return this.Unauthorized();
            }

            this.helper.Delete(trickId);
            return this.RedirectToAction(nameof(this.All));
        }

        [Authorize]
        public IActionResult Search(string searchCriteria) 
            => this.View(nameof(this.All), this.helper.Search(searchCriteria));
    }
}
