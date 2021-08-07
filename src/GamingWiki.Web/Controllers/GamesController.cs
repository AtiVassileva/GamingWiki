using System.Collections.Generic;
using AutoMapper;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Games;
using GamingWiki.Web.Models;
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
        private const int GamesPerPage = 3;

        private readonly IGameService helper;
        private readonly IMapper mapper;
        public GamesController(IGameService helper, IMapper mapper)
        {
            this.helper = helper;
            this.mapper = mapper;
        }

        public IActionResult All(int pageIndex = 1) 
            => this.View(new GameFullModel
            {
                Games = PaginatedList<GameServiceListingModel>
                    .Create(this.helper.All(), pageIndex, GamesPerPage),
                Genres = this.helper.GetGenres(),
                Tokens = new KeyValuePair<object, object>
                    ("All", null)
            });
        
        public IActionResult Create() 
            => this.View(new GameFormModel
            {
                Areas = this.helper.GetAreas(),
                Genres = this.helper.GetGenres(),
                Platforms = this.helper.GetPlatforms()
            });

        [HttpPost]
        public IActionResult Create(GameFormModel model)
        {
            if (this.helper.GameExists(model.Name))
            {
                this.ModelState.AddModelError(nameof(model.AreaId), AlreadyExistingGameExceptionMessage);
            }

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
                model.GenreId, model.CreatorsNames,
                model.SupportedPlatforms);

            return this.RedirectToAction(nameof(this.Details),
                new { gameId });
        }
        
        public IActionResult Details(int gameId)
            => this.helper.GameExists(gameId)
                ? this.View(this.helper.Details(gameId))
                : this.View("Error", CreateError(NonExistingGameExceptionMessage));
        
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
            viewModel.SupportedPlatforms = this.helper
                .GetGamePlatforms(gameId);
            viewModel.AllPlatforms = this.helper.GetPlatforms();

            return this.View(viewModel);
        }

        [HttpPost]
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

            var edited = this.helper.Edit(gameId, model.Description, model.PictureUrl, model.AreaId, model.TrailerUrl, model.SupportedPlatforms);

            if (!edited)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(this.Details),
                new { gameId });
        }

        public IActionResult Delete(int gameId)
        {
            if (!this.helper.GameExists(gameId))
            {
                return this.View("Error", CreateError(NonExistingGameExceptionMessage));
            }

            var deleted = this.helper.Delete(gameId);

            if (!deleted)
            {
                return this.BadRequest();
            }

            return this.Redirect(nameof(this.All));
        }

        public IActionResult Search([FromQuery(Name = "parameter") ]
            string letter, int pageIndex = 1)
            => this.View(nameof(this.All), new GameFullModel
            {
                Games = PaginatedList<GameServiceListingModel>
                    .Create(this.helper.Search(letter), 
                        pageIndex, GamesPerPage),
                Genres = this.helper.GetGenres(),
                Tokens = new KeyValuePair<object, object>
                    ("Search", letter)
            });

        public IActionResult Filter([FromQuery(Name = "parameter")]
            int genreId, int pageIndex = 1)
            => this.View(nameof(this.All), new GameFullModel
            {
                Games = PaginatedList<GameServiceListingModel>
                    .Create(this.helper.Filter(genreId), 
                        pageIndex, GamesPerPage),
                Genres = this.helper.GetGenres(),
                Tokens = new KeyValuePair<object, object>
                ("Filter", genreId)
            });

    }
}
