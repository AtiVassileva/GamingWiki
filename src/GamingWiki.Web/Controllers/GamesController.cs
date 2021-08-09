using System.Collections.Generic;
using AutoMapper;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Games;
using GamingWiki.Web.Infrastructure;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Games;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Common.ExceptionMessages;
using static GamingWiki.Web.Common.AlertMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    [Authorize]
    public class GamesController : Controller
    {
        private const int GamesPerPage = 3;

        private readonly IGameService gameService;
        private readonly IMapper mapper;

        public GamesController(IGameService helper, IMapper mapper)
        {
            this.gameService = helper;
            this.mapper = mapper;
        }

        public IActionResult All(int pageIndex = 1) 
            => this.View(new GameFullModel
            {
                Games = PaginatedList<GameServiceListingModel>
                    .Create(this.gameService.All(approvedOnly: 
                        !this.User.IsAdmin()), pageIndex, GamesPerPage),
                Genres = this.gameService.GetGenres(),
                Tokens = new KeyValuePair<object, object>
                    ("All", null)
            });
        
        public IActionResult Create() 
            => this.View(new GameFormModel
            {
                Areas = this.gameService.GetAreas(),
                Genres = this.gameService.GetGenres(),
                Platforms = this.gameService.GetPlatforms()
            });

        [HttpPost]
        public IActionResult Create(GameFormModel model)
        {
            if (this.gameService.GameExists(model.Name))
            {
                this.ModelState.AddModelError(nameof(model.AreaId), AlreadyExistingGameExceptionMessage);
            }

            if (!this.gameService.AreaExists(model.AreaId))
            {
                this.ModelState.AddModelError(nameof(model.AreaId), NonExistingAreaExceptionMessage);
            }

            if (!this.gameService.GenreExists(model.GenreId))
            {
                this.ModelState.AddModelError(nameof(model.GenreId), NonExistingGenreExceptionMessage);
            }

            if (!this.ModelState.IsValid)
            {
                model.Areas = this.gameService.GetAreas();
                model.Genres = this.gameService.GetGenres();
                model.Platforms = this.gameService.GetPlatforms();

                return this.View(model);
            }

            var gameId = this.gameService.Create(model.Name, model.PictureUrl, model.TrailerUrl, model.Description, model.AreaId, 
                model.GenreId, model.CreatorsNames,
                contributorId:this.User.GetId(),
                isApproved: this.User.IsAdmin(),
                model.SupportedPlatforms);

            TempData[GlobalMessageKey] = this.User.IsAdmin()
                ? SuccessfullyAddedGameAdminMessage
                : SuccessfullyAddedGameUserMessage;

            return this.RedirectToAction(nameof(this.Details),
                new { gameId });
        }
        
        public IActionResult Details(int gameId)
            => this.gameService.GameExists(gameId)
                ? this.View(this.gameService.Details(gameId))
                : this.View("Error", CreateError(NonExistingGameExceptionMessage));
        
        public IActionResult Edit(int gameId)
        {
            if (!this.gameService.GameExists(gameId))
            {
                return this.View("Error", CreateError(NonExistingGameExceptionMessage));
            }

            var dbModel = this.gameService.Details(gameId);

            var viewModel = this.mapper
                .Map<GameServiceEditModel>(dbModel);

            viewModel.Areas = this.gameService.GetAreas();
            viewModel.SupportedPlatforms = this.gameService
                .GetGamePlatforms(gameId);
            viewModel.AllPlatforms = this.gameService.GetPlatforms();

            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(GameServiceEditModel model, int gameId)
        {
            if (!this.gameService.AreaExists(model.AreaId))
            {
                this.ModelState.AddModelError(nameof(model.AreaId), NonExistingAreaExceptionMessage);
            }

            if (!this.ModelState.IsValid)
            {
                var dbModel = this.gameService.Details(gameId);

                model = this.mapper.Map<GameServiceEditModel>(dbModel);
                model.Areas = this.gameService.GetAreas();

                return this.View(model);
            }

            model.IsApproved = this.User.IsAdmin();

            var edited = this.gameService.Edit(gameId, model);

            if (!edited)
            {
                return this.BadRequest();
            }

            TempData[GlobalMessageKey] = this.User.IsAdmin()
                ? SuccessfullyEditedGameAdminMessage
                : SuccessfullyEditedGameUserMessage;

            return this.RedirectToAction(nameof(this.Details),
                new { gameId });
        }

        public IActionResult Delete(int gameId)
        {
            if (!this.gameService.GameExists(gameId))
            {
                return this.View("Error", CreateError(NonExistingGameExceptionMessage));
            }

            var deleted = this.gameService.Delete(gameId);

            if (!deleted)
            {
                return this.BadRequest();
            }

            TempData[GlobalMessageKey] =  DeletedGameMessage;

            return this.Redirect(nameof(this.All));
        }

        public IActionResult Search([FromQuery(Name = "parameter") ]
            string letter, int pageIndex = 1)
            => this.View(nameof(this.All), new GameFullModel
            {
                Games = PaginatedList<GameServiceListingModel>
                    .Create(this.gameService.Search(letter), 
                        pageIndex, GamesPerPage),
                Genres = this.gameService.GetGenres(),
                Tokens = new KeyValuePair<object, object>
                    ("Search", letter)
            });

        public IActionResult Filter([FromQuery(Name = "parameter")]
            int genreId, int pageIndex = 1)
            => this.View(nameof(this.All), new GameFullModel
            {
                Games = PaginatedList<GameServiceListingModel>
                    .Create(this.gameService.Filter(genreId), 
                        pageIndex, GamesPerPage),
                Genres = this.gameService.GetGenres(),
                Tokens = new KeyValuePair<object, object>
                ("Filter", genreId)
            });

        public IActionResult Mine(int pageIndex = 1)
            => this.View(nameof(this.All), new GameFullModel
            {
                Games = PaginatedList<GameServiceListingModel>
                    .Create(this.gameService.Mine(this.User.GetId()),
                        pageIndex, GamesPerPage),
                Genres = this.gameService.GetGenres(),
                Tokens = new KeyValuePair<object, object>("Mine", null)
            });

    }
}
