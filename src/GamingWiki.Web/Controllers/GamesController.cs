using GamingWiki.Services.Contracts;
using GamingWiki.Web.Models.Games;
using static GamingWiki.Web.Common.WebConstants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameService helper;

        public GamesController(IGameService helper)
            => this.helper = helper;

        [Authorize]
        public IActionResult All() 
            => this.View(new GameFullModel
            {
                Games = this.helper.All(),
                Genres = this.helper.GetGenres(),
            });

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Create() 
            => this.View(new GameFormModel
            {
                Areas = this.helper.GetAreas(),
                Genres = this.helper.GetGenres(),
                Platforms = this.helper.GetPlatforms()
            });

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Create(GameFormModel model)
        {
            if (!this.helper.AreaExists(model.AreaId))
            {
                this.ModelState.AddModelError(nameof(model.AreaId), "Area does not exist.");
            }

            if (!this.helper.GenreExists(model.GenreId))
            {
                this.ModelState.AddModelError(nameof(model.GenreId), "Genre does not exist.");
            }

            if (!this.ModelState.IsValid)
            {
                model.Areas = this.helper.GetAreas();
                model.Genres = this.helper.GetGenres();
                model.Platforms = this.helper.GetPlatforms();

                return this.View(model);
            }

            var gameId = this.helper.Create(model.Name, model.PictureUrl, model.TrailerUrl, model.Description, model.AreaId, 
                model.GenreId, model.CreatorsNames);

            return this.RedirectToAction(nameof(this.Details),
                new { gameId = $"{gameId}" });
        }

        [Authorize]
        public IActionResult Details(int gameId)
            => this.helper.GameExists(gameId)
                ? this.View(this.helper.Details(gameId))
                : this.View("Error");

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int gameId)
        {
            if (!this.helper.GameExists(gameId))
            {
                return this.View("Error");
            }

            var dbModel = this.helper.Details(gameId);

            var viewModel = new GameEditModel
            {
                Name = dbModel.Name,
                PictureUrl = dbModel.PictureUrl,
                TrailerUrl = dbModel.TrailerUrl,
                Description = dbModel.Description,
                AreaId = dbModel.AreaId,
                Area = dbModel.Area,
                Areas = this.helper.GetAreas()
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(GameEditModel model, int gameId)
        {
            if (!this.helper.AreaExists(model.AreaId))
            {
                this.ModelState.AddModelError(nameof(model.AreaId), "Area does not exist.");
            }

            if (!this.ModelState.IsValid)
            {
                var dbModel = this.helper.Details(gameId);

                model = new GameEditModel
                {
                    Name = dbModel.Name,
                    PictureUrl = dbModel.PictureUrl,
                    TrailerUrl = dbModel.TrailerUrl,
                    Description = dbModel.Description,
                    Areas = this.helper.GetAreas()
                };

                return this.View(model);
            }

            this.helper.Edit(gameId, model.Description, model.PictureUrl, model.AreaId, model.TrailerUrl);

            return this.RedirectToAction(nameof(this.Details),
                new { gameId = $"{gameId}" });
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Delete(int gameId)
        {
            if (!this.helper.GameExists(gameId))
            {
                return this.View("Error");
            }

            this.helper.Delete(gameId);
            return this.Redirect(nameof(this.All));
        }

        [Authorize]
        public IActionResult Search(string letter) 
            => this.View(nameof(this.All), new GameFullModel
            {
                Games = this.helper.Search(letter),
                Genres = this.helper.GetGenres()
            });

        [Authorize]
        public IActionResult Filter(int genreId)
            => this.View(nameof(this.All), new GameFullModel
            {
                Games = this.helper.Filter(genreId),
                Genres = this.helper.GetGenres()
            });

    }
}
