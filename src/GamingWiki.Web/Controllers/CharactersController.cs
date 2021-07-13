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

        public IActionResult All()
        {
            var characterModels = this.dbContext
                .Characters.Select(c => new CharacterSimpleModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    PictureUrl = c.PictureUrl,
                }).ToList();

            return this.View(characterModels);
        }

        public IActionResult Create()
        {
            var gameNames = GetGameNames();

            var characterClasses = GetCharacterClasses();

            var collectionModel = new CharacterCollectionsModel
            {
                GameNames = gameNames,
                CharacterClasses = characterClasses
            };

            return this.View(collectionModel);
        }

        [HttpPost]
        public IActionResult Create(CharacterFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Error", new ErrorViewModel
                {
                    RequestId = Guid.NewGuid().ToString()
                });
            }

            var characterDto = new CharacterDtoModel
            {
                Name = model.Name,
                PictureUrl = model.PictureUrl,
                Description = model.Description,
                ClassId = 2,
                GameId = this.helper.ParseGame(model.Game).Id
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
                    Class = c.Class.ToString(),
                    Game = c.Game.Name,
                    GameId = c.Game.Id
                }).FirstOrDefault();

            return this.View(characterModel);
        }

        public IActionResult Edit(int characterId)
        {
            var characterModel = this.dbContext.Characters
                .Where(c => c.Id == characterId)
                .Select(c => new CharacterEditModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    PictureUrl = c.PictureUrl,
                    Description = c.Description
                }).FirstOrDefault();


            return this.View(characterModel);
        }

        [HttpPost]
        public IActionResult Edit(CharacterEditModel model, int characterId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Error", new ErrorViewModel
                {
                    RequestId = Guid.NewGuid().ToString()
                });
            }

            var character = this.dbContext.Characters
                .First(c => c.Id == characterId);

            character.PictureUrl = model.PictureUrl;
            character.Description = model.Description;

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

        private static IEnumerable<string> GetCharacterClasses()
        {
            return null;
        }

        private IEnumerable<string> GetGameNames()
        {
            return this.dbContext.Games
                .Select(g => g.Name).ToList();
        }
    }
}
