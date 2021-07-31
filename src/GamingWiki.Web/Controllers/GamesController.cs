using AutoMapper;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Games;
using GamingWiki.Web.Models.Games;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Common.ExceptionMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    [Authorize]
    public class GamesController : Controller
    {
        private readonly IGameService helper;
        private readonly IMapper mapper;
        public GamesController(IGameService helper, IMapper mapper)
        {
            this.helper = helper;
            this.mapper = mapper;
        }

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
                this.ModelState.AddModelError(nameof(model.AreaId), NonExistingAreaExceptionMessage);
            }

            if (!this.helper.GenreExists(model.GenreId))
            {
                this.ModelState.AddModelError(nameof(model.GenreId), NonExistingGenreExceptionMessage);
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
        
        public IActionResult Details(int gameId)
            => this.helper.GameExists(gameId)
                ? this.View(this.helper.Details(gameId))
                : this.View("Error", CreateError(NonExistingGameExceptionMessage));

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int gameId)
        {
            if (!this.helper.GameExists(gameId))
            {
                return this.View("Error", CreateError(NonExistingGameExceptionMessage));
            }

            var dbModel = this.helper.Details(gameId);

            var viewModel = this.mapper
                .Map<GameServiceEditModel>(dbModel);

            viewModel.Areas = this.helper.GetAreas();

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(GameServiceEditModel model, int gameId)
        {
            if (!this.helper.AreaExists(model.AreaId))
            {
                this.ModelState.AddModelError(nameof(model.AreaId), NonExistingAreaExceptionMessage);
            }

            if (!this.ModelState.IsValid)
            {
                var dbModel = this.helper.Details(gameId);

                model = this.mapper.Map<GameServiceEditModel>(dbModel);
                model.Areas = this.helper.GetAreas();

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
                return this.View("Error", CreateError(NonExistingGameExceptionMessage));
            }

            this.helper.Delete(gameId);
            return this.Redirect(nameof(this.All));
        }
        
        public IActionResult Search(string letter) 
            => this.View(nameof(this.All), new GameFullModel
            {
                Games = this.helper.Search(letter),
                Genres = this.helper.GetGenres()
            });
        
        public IActionResult Filter(int genreId)
            => this.View(nameof(this.All), new GameFullModel
            {
                Games = this.helper.Filter(genreId),
                Genres = this.helper.GetGenres()
            });

    }
}
