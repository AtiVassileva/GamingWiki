using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GamingWiki.Data;
using GamingWiki.Services;
using GamingWiki.Services.Contracts;
using GamingWiki.Web.Models.Areas;
using GamingWiki.Web.Models.Characters;
using GamingWiki.Web.Models.Games;
using GamingWiki.Web.Models.Genres;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IGameService helper;
        private readonly IMapper mapper;

        public GamesController
            (ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.helper = new GameService(dbContext);
        }

        [Authorize]
        public IActionResult All()
        {
            var gamesModels = this.dbContext.Games
                .Select(g => new GameViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    PictureUrl = g.PictureUrl,
                }).OrderBy(g => g.Name)
                .ToList();

            return this.View(new GameFullModel
            {
                Games = gamesModels,
                Genres = this.GetGenres()
            });
        }

        [Authorize]
        public IActionResult Create() 
            => this.View(new GameFormModel
            {
                Areas = this.GetAreas(),
                Genres = this.GetGenres(),
            });

        [Authorize]
        [HttpPost]
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
                model.Areas = this.GetAreas();
                model.Genres = this.GetGenres();

                return this.View(model);
            }

            var id = this.helper.Create(model.Name, model.PictureUrl, model.TrailerUrl, model.Description, model.AreaId, 
                model.GenreId, model.CreatorsNames);

            return this.Redirect($"/Games/Details?gameId={id}");
        }

        public IActionResult Details(int gameId)
        {
            var gameModel = this.dbContext.Games
                .Where(g => g.Id == gameId)
                .Select(g => new GameListingModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    PictureUrl = g.PictureUrl,
                    TrailerUrl = g.TrailerUrl,
                    Description = g.Description,
                    Area = g.Area.Name,
                    Genre = g.Genre.Name,
                    Ratings = this.helper.GetRatings(g.Id),
                    Rating = this.helper.GetRatings(g.Id).Values.Average(),
                    Creators = g.GamesCreators.Where(gc => gc.GameId == g.Id).Select(gc => gc.Creator.Name).ToList(),
                    Characters = this.dbContext.Characters
                        .Where(c => c.GameId == gameId)
                        .Select(c => new CharacterGameModel
                        {
                            Id = c.Id,
                            Name = c.Name
                        })
                        .OrderBy(c => c.Name)
                        .ToList()
                }).FirstOrDefault();

            return this.View(gameModel);
        }

        public IActionResult Edit(int gameId)
        {
            var entity = this.dbContext.Games
                .First(g => g.Id == gameId);

            var viewModel = new GameEditModel
            {
                Id = entity.Id,
                Name = entity.Name,
                PictureUrl = entity.PictureUrl,
                TrailerUrl = entity.TrailerUrl,
                Description = entity.Description,
                Areas = this.GetAreas()
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(GameEditModel model, int gameId)
        {
            if (!this.helper.AreaExists(model.AreaId))
            {
                this.ModelState.AddModelError(nameof(model.AreaId), "Area does not exist.");
            }

            if (!this.ModelState.IsValid)
            {
                model.Areas = this.GetAreas();
                return this.View(model);
            }

            var game = this.dbContext.Games
                .First(g => g.Id == gameId);

            game.Description = model.Description;
            game.PictureUrl = model.PictureUrl;
            game.AreaId = model.AreaId;
            game.TrailerUrl = model.TrailerUrl;

            this.dbContext.SaveChanges();

            return this.Redirect($"/Games/Details?gameId={game.Id}");
        }

        public IActionResult Delete(int gameId)
        {
            var game = this.dbContext.Games
                .First(g => g.Id == gameId);

            this.dbContext.Games.Remove(game);
            this.dbContext.SaveChanges();

            return this.Redirect("/Games/All");
        }

        public IActionResult Search(string letter)
        {
            var gamesModels =  this.dbContext
                .Games
                .Where(g => g.Name.ToUpper()
                    .StartsWith(letter))
                .Select(g => new GameViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    PictureUrl = g.PictureUrl,
                }).ToList();

            return this.View("All", new GameFullModel
            {
                Games = gamesModels,
                Genres = this.GetGenres()
            });
        }

        public IActionResult Filter(int genreId)
        {
            var matchingGames = this.dbContext
                .Games.Where(g => g.GenreId == genreId)
                .Select(g => new GameViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    PictureUrl = g.PictureUrl
                }).ToList();

            return this.View("All", new GameFullModel
            {
                Games = matchingGames,
                Genres = this.GetGenres()
            });
        }
        private IEnumerable<GenreViewModel> GetGenres()
            => this.dbContext
                .Genres
                .Select(g => 
                    new GenreViewModel
                {
                Id = g.Id,
                Name = g.Name
                })
                .OrderBy(g => g.Name)
                .ToList();

        private IEnumerable<AreaViewModel> GetAreas()
            => this.dbContext
                .Areas
                .Select(a => 
                    new AreaViewModel
                {
                    Id = a.Id,
                    Name = a.Name
                }).ToList();

        
    }
}
