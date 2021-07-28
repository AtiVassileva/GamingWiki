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
                this.ModelState.AddModelError(nameof(model.GameId), "Game does not exist.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var authorId = this.User.GetId();

            this.helper.Create(model.Heading, model.Content, authorId, model.PictureUrl, model.GameId);

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
