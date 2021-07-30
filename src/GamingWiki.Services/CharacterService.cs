using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Characters;
using GamingWiki.Services.Models.Classes;
using GamingWiki.Services.Models.Games;
using static GamingWiki.Services.Common.ExceptionMessages;

namespace GamingWiki.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public CharacterService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public Game ParseGame(string gameName)
            => this.dbContext.Games.First(g => g.Name == gameName);

        public bool GameExists(int gameId)
            => this.dbContext.Games.Any(g => g.Id == gameId);

        public bool ClassExists(int classId)
            => this.dbContext.Classes.Any(c => c.Id == classId);

        public bool CharacterExists(int characterId)
            => this.dbContext.Characters.Any(c => c.Id == characterId);

        public int Create(string name, string pictureUrl,
            string description, int classId, int gameId)
        {
            var character = new Character
            {
                Name = name,
                PictureUrl = pictureUrl,
                Description = description,
                ClassId = classId,
                GameId = gameId
            };

            this.dbContext.Characters.Add(character);
            this.dbContext.SaveChanges();

            return character.Id;
        }

        public CharacterServiceDetailsModel Details(int characterId)
        {
            var character = this.FindCharacter(characterId);

            var detailsModel = this.mapper
                .Map<CharacterServiceDetailsModel>(character);

            return detailsModel;
        }

        public IEnumerable<CharacterAllServiceModel> All()
        => this.dbContext.Characters
                .Select(c => this.mapper
                    .Map<CharacterAllServiceModel>(c))
                .ToList();

        public void Edit(int characterId, CharacterServiceEditModel model)
        {
            if (!this.CharacterExists(characterId))
            {
                throw new InvalidOperationException(NonExistingCharacterExceptionMessage);
            }

            var character = this.FindCharacter(characterId);

            character.PictureUrl = model.PictureUrl;
            character.Description = model.Description;
            character.ClassId = model.ClassId;

            this.dbContext.SaveChanges();
        }

        public void Delete(int characterId)
        {
            if (!this.CharacterExists(characterId))
            {
                throw new InvalidOperationException(NonExistingCharacterExceptionMessage);
            }

            var character = this.FindCharacter(characterId);

            this.dbContext.Characters.Remove(character);
            this.dbContext.SaveChanges();
        }

        public IEnumerable<CharacterAllServiceModel> Search(string letter)
            => this.dbContext.Characters
                .Where(c => c.Name.ToUpper()
                    .StartsWith(letter))
                .Select(c => this.mapper
                    .Map<CharacterAllServiceModel>(c))
                .OrderBy(c => c.Name)
                .ToList();

        public IEnumerable<CharacterAllServiceModel> Filter(int classId)
            => this.dbContext.Characters
                .Where(c => c.ClassId == classId)
                .Select(c => this.mapper
                    .Map<CharacterAllServiceModel>(c))
                .ToList();

        public IEnumerable<ClassSimpleServiceModel> GetClasses()
            => this.dbContext.Classes
                .Select(c => this.mapper
                    .Map<ClassSimpleServiceModel>(c))
                .ToList();

        public IEnumerable<GameServiceSimpleModel> GetGames()
        => this.dbContext.Games
            .Select(g => this.mapper
                .Map<GameServiceSimpleModel>(g))
            .ToList();

        private Character FindCharacter(int characterId)
            => this.dbContext.Characters
                .First(c => c.Id == characterId);
    }
}
