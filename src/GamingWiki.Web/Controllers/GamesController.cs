using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Services;
using GamingWiki.Services.Contracts;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Areas;
using GamingWiki.Web.Models.Games;
using GamingWiki.Web.Models.Genres;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IGameHelper helper;
        private readonly IMapper mapper;

        public GamesController
            (ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.helper = new GameHelper(dbContext);
        }

        public IActionResult All() 
            => this.View(this.dbContext.Games
                .Select(g => new GameSimpleModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    PictureUrl = g.PictureUrl,
                }).ToList());

        public IActionResult Create() 
            => this.View(new GameFormModel
            {
                Areas = this.GetAreas(),
                Genres = this.GetGenres(),
            });

        
        [HttpPost]
        public IActionResult Create(GameFormModel model)
        {
            if (!this.dbContext.Areas.Any(a => a.Id == model.AreaId))
            {
                this.ModelState.AddModelError(nameof(model.AreaId), "Area does not exist.");
            }

            if (!this.dbContext.Genres.Any(g => g.Id == model.GenreId))
            {
                this.ModelState.AddModelError(nameof(model.GenreId), "Genre does not exist.");
            }

            if (!this.ModelState.IsValid)
            {
                model.Areas = this.GetAreas();
                model.Genres = this.GetGenres();

                return this.View(model);
            }

            var gameDto = new GameDtoModel
            {
                Name = model.Name,
                Description = model.Description,
                PictureUrl = model.PictureUrl,
                TrailerUrl = model.TrailerUrl,
                AreaId = model.AreaId,
                GenreId= model.GenreId
            };

            var game = mapper.Map<Game>(gameDto);

            this.dbContext.Games.Add(game);
            this.dbContext.SaveChanges();

            var creators = this.helper.ParseCreators(model.CreatorsNames);

            foreach (var creator in creators)
            {
                var gameCreatorDto = new GameCreatorDto
                {
                    GameId = game.Id,
                    CreatorId = creator.Id
                };

                var gameCreator = this.mapper.Map<GameCreator>(gameCreatorDto);

                this.dbContext.GamesCreators.Add(gameCreator);
                this.dbContext.SaveChanges();
            }

            return this.Redirect($"/Games/Details?gameId={game.Id}");
        }

        public IActionResult Details(int gameId) 
            => this.View(this.dbContext.Games
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
                    Creators = g.GamesCreators.Where(gc => gc.GameId == g.Id).Select(gc => gc.Creator.Name).ToList()
                }).FirstOrDefault());

        public IActionResult Edit(int gameId)
        {
            var dbModel = this.dbContext.Games.First(g => g.Id == gameId);

            var viewModel = new GameEditModel
            {
                Id = dbModel.Id,
                Name = dbModel.Name,
                PictureUrl = dbModel.PictureUrl,
                TrailerUrl = dbModel.TrailerUrl,
                Description = dbModel.Description,
                AreaId = dbModel.AreaId,
                Areas = this.GetAreas(),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(GameEditModel model, int gameId)
        {
            if (!this.dbContext.Areas.Any(a => a.Id == model.AreaId))
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
            => this.View("All", this.dbContext
                    .Games
                    .Where(g => g.Name.ToUpper()
                    .StartsWith(letter))
                    .Select(g => new GameSimpleModel
                    {
                    Id = g.Id,
                    Name = g.Name,
                    PictureUrl = g.PictureUrl,
                    })
                    .ToList());

        private IEnumerable<GenreViewModel> GetGenres()
            => this.dbContext
                .Genres
                .Select(g => 
                    new GenreViewModel
                {
                Id = g.Id,
                Name = g.Name
                }).ToList();

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
