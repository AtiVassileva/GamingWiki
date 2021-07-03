using System;
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
        private readonly IEntityHelper helper;
        private readonly IMapper mapper;

        public GamesController
            (ApplicationDbContext dbContext, IMapper mapper, IEntityHelper creator)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.helper = new EntityHelper(dbContext);
        }

        public IActionResult All()
        {
            var gameModels = this.dbContext.Games
                .Select(g => new GameSimpleModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    PictureUrl = g.PictureUrl,
                    Creators = string.Join(", ", g.GamesCreators.Select(gc => gc.Creator.Name))
                }).ToList();

            return this.View(gameModels);
        }

        public IActionResult Create()
        {
            var placesModels = Enum.GetValues<PlaceType>()
                .Select(en => 
                    new string(en.ToString())).ToList();

            return this.View(placesModels);
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
                PlaceId = place.Id
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
                    Description = g.Description,
                    Place = $"{g.Place.Name} ({g.Place.PlaceType.ToString()})",
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
                    Description = g.Description,
                    Place = g.Place.Name,
                }).FirstOrDefault();

            ViewBag.PlaceTypes = Enum.GetValues<PlaceType>()
                .Select(en =>
                    new string(en.ToString())).ToList();

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
    }
}
