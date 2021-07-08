using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Models.Enums;
using GamingWiki.Services;
using GamingWiki.Services.Contracts;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Games;
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
        {
            var gameModels = this.dbContext.Games
                .Select(g => new GameSimpleModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    PictureUrl = g.PictureUrl,
                }).ToList();

            return this.View(gameModels);
        }

        public IActionResult Create()
        {
            var placeTypes = GetPlaceTypes();
            var gamesClasses = GetGamesClasses();

            var collectionsModel = new GameCollectionsModel
            {
                PlaceTypes = placeTypes,
                GameClasses = gamesClasses
            };
            return this.View(collectionsModel);
        }

        [HttpPost]
        public IActionResult Create(GameFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Error", new ErrorViewModel
                {
                    RequestId = Guid.NewGuid().ToString()
                });
            }

            var place = this.helper.ParsePlace(model.PlaceName, model.PlaceType);

            var gameDto = new GameDtoModel
            {
                Name = model.Name,
                Description = model.Description,
                PictureUrl = model.PictureUrl,
                TrailerUrl = model.TrailerUrl,
                PlaceId = place.Id,
                Class = Enum.Parse<GameClass>(model.Class)
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
                    Place = $"{g.Place.Name} ({g.Place.PlaceType})",
                    Class = g.Class.ToString(),
                    Creators = g.GamesCreators.Where(gc => gc.GameId == g.Id).Select(gc => gc.Creator.Name).ToList()
                }).FirstOrDefault();

            return this.View(gameModel);
        }

        public IActionResult Edit(int gameId)
        {
            var game = this.dbContext.Games
                .Where(g => g.Id == gameId)
                .Select(g => new GameListingModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    PictureUrl = g.PictureUrl,
                    TrailerUrl = g.TrailerUrl,
                    Description = g.Description,
                    Place = g.Place.Name,
                    Class = g.Class.ToString()
                }).FirstOrDefault();

            ViewBag.PlaceTypes = GetPlaceTypes();

            return this.View(game);
        }

        [HttpPost]
        public IActionResult Edit(GameEditModel model, int gameId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Error", new ErrorViewModel
                {
                    RequestId = Guid.NewGuid().ToString()
                });
            }

            var game = this.dbContext.Games
                .First(g => g.Id == gameId);

            game.Description = model.Description;
            game.PictureUrl = model.PictureUrl;
            game.PlaceId = this.helper.ParsePlace(model.PlaceName, model.PlaceType).Id;
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
            var gameModels = this.dbContext.Games
                .Where(g => g.Name.ToUpper()
                    .StartsWith(letter))
                .Select(g => new GameSimpleModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    PictureUrl = g.PictureUrl,
                }).ToList();

            return this.View("All", gameModels);
        }

        private static IEnumerable<string> GetPlaceTypes()
        {
            return Enum.GetValues<PlaceType>()
                .Select(en =>
                   new string(en.ToString())).ToList();
        }

        private static IEnumerable<string> GetGamesClasses()
        {
            return Enum.GetValues<GameClass>()
                .Select(gc =>
                    new string(gc.ToString())).ToList();
        }
    }
}
