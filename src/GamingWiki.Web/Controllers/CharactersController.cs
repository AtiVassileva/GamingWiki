using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Services;
using GamingWiki.Services.Contracts;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Characters;
using GamingWiki.Web.Models.Games;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    public class CharactersController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ICharacterHelper helper;
        private readonly IMapper mapper;

        public CharactersController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.helper = new CharacterHelper(dbContext);
        }

        public IActionResult All() =>
            this.View(this.dbContext
                .Characters.Select(c => new CharacterSimpleModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    PictureUrl = c.PictureUrl,
                }).ToList());

        public IActionResult Create() =>
            this.View(new CharacterFormModel
            {
                Classes = this.GetClasses(),
                Games = this.GetGames()
            });

        [HttpPost]
        public IActionResult Create(CharacterFormModel model)
        {
            if (!this.dbContext.Games.Any(g => g.Id == model.GameId))
            {
                this.ModelState.AddModelError(nameof(model.GameId), "Game does not exist.");
            }

            if (!this.dbContext.Classes.Any(c => c.Id == model.ClassId))
            {
                this.ModelState.AddModelError(nameof(model.ClassId), "Class does not exist.");
            }

            if (!this.ModelState.IsValid)
            {
                model.Games = this.GetGames();
                model.Classes = this.GetClasses();

                return this.View(model);
            }

            var characterDto = new CharacterDtoModel
            {
                Name = model.Name,
                PictureUrl = model.PictureUrl,
                Description = model.Description,
                ClassId = model.ClassId,
                GameId = model.GameId
            };

            var character = this.mapper.Map<Character>(characterDto);

            this.dbContext.Characters.Add(character);
            this.dbContext.SaveChanges();

            return this.Redirect($"/Characters/Details?characterId={character.Id}");
        }

        public IActionResult Details(int characterId)
        {
            var characterModel = this.dbContext.Characters
                .Where(c => c.Id == characterId)
                .Select(c => new CharacterListingModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    PictureUrl = c.PictureUrl,
                    Description = c.Description,
                    Class = c.Class.Name,
                    Game = c.Game.Name,
                    GameId = c.Game.Id
                }).FirstOrDefault();

            return this.View(characterModel);
        }

        public IActionResult Edit(int characterId)
        {
            var dbModel = this.dbContext.Characters
                .First(c => c.Id == characterId);

                var characterModel = new CharacterEditModel
                {
                    Id = dbModel.Id,
                    Name = dbModel.Name,
                    PictureUrl = dbModel.PictureUrl,
                    Description = dbModel.Description,
                    Classes = this.GetClasses()
                };

                return this.View(characterModel);
        }

        [HttpPost]
        public IActionResult Edit(CharacterEditModel model, int characterId)
        {
            if (!this.dbContext.Classes.Any(c => c.Id == model.ClassId))
            {
                this.ModelState.AddModelError(nameof(model.ClassId), "Class does not exist.");
            }

            if (!this.ModelState.IsValid)
            {
                model.Classes = this.GetClasses();

                return this.View(model);
            }

            var character = this.dbContext.Characters
                .First(c => c.Id == characterId);

            character.PictureUrl = model.PictureUrl;
            character.Description = model.Description;
            character.ClassId = model.ClassId;

            this.dbContext.SaveChanges();

            return this.Redirect($"/Characters/Details?characterId={characterId}");
        }

        public IActionResult Delete(int characterId)
        {
            var character = this.dbContext.Characters
                .First(c => c.Id == characterId);

            this.dbContext.Characters.Remove(character);
            this.dbContext.SaveChanges();

            return this.Redirect("/Characters/All");
        }

        public IActionResult Search(string letter)
        {
            var characterModels = this.dbContext.Characters
                .Where(c => c.Name.ToUpper()
                    .StartsWith(letter))
                .Select(c => new CharacterSimpleModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    PictureUrl = c.PictureUrl,
                }).ToList();

            return this.View("All", characterModels);
        }

        private IEnumerable<ClassViewModel> GetClasses() =>
            this.dbContext.Classes
                .Select(c => new ClassViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();

        private IEnumerable<GameSimpleModel> GetGames() =>
            this.dbContext.Games
                .Select(g => new GameSimpleModel
                {
                    Id = g.Id,
                    Name = g.Name
                }).ToList();
    }
}
