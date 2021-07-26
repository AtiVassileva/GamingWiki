using System.Collections.Generic;
using System.Linq;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Characters;
using GamingWiki.Services.Models.Classes;
using GamingWiki.Services.Models.Games;

namespace GamingWiki.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly ApplicationDbContext dbContext;

        public CharacterService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Game ParseGame(string gameName)
            => this.dbContext.Games.First(g => g.Name == gameName);

        public bool GameExists(int gameId)
            => this.dbContext.Games.Any(g => g.Id == gameId);

        public bool ClassExists(int classId)
            => this.dbContext.Classes.Any(c => c.Id == classId);

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
            => this.dbContext.Characters
                .Where(c => c.Id == characterId)
                .Select(c => new CharacterServiceDetailsModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    PictureUrl = c.PictureUrl,
                    Description = c.Description,
                    Class = c.Class.Name,
                    Game = c.Game.Name,
                    GameId = c.Game.Id
                }).FirstOrDefault();

        public IEnumerable<CharacterAllServiceModel> All()
        => this.dbContext.Characters
                .Select(c => new CharacterAllServiceModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    PictureUrl = c.PictureUrl,
                })
                .OrderBy(c => c.Name)
                .ToList();

        public void Edit(int characterId, CharacterServiceEditModel model)
        {
            var character = this.dbContext.Characters
                .FirstOrDefault(c => c.Id == characterId);

            if (character == null)
            {
                return;
            }

            character.PictureUrl = model.PictureUrl;
            character.Description = model.Description;
            character.ClassId = model.ClassId;

            this.dbContext.SaveChanges();
        }

        public void Delete(int characterId)
        {
            var character = this.dbContext.Characters
                .FirstOrDefault(c => c.Id == characterId);

            if (character == null)
            {
                return;
            }

            this.dbContext.Characters.Remove(character);
            this.dbContext.SaveChanges();
        }

        public IEnumerable<CharacterAllServiceModel> Search(string letter)
            => this.dbContext.Characters
                .Where(c => c.Name.ToUpper()
                    .StartsWith(letter))
                .Select(c => new CharacterAllServiceModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    PictureUrl = c.PictureUrl,
                })
                .OrderBy(c => c.Name)
                .ToList();

        public IEnumerable<CharacterAllServiceModel> Filter(int classId)
            => this.dbContext.Characters
                .Where(c => c.ClassId == classId)
                .Select(c => new CharacterAllServiceModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    PictureUrl = c.PictureUrl
                })
                .OrderBy(c => c.Name)
                .ToList();

        public IEnumerable<ClassSimpleServiceModel> GetClasses()
            => this.dbContext.Classes
                .Select(c => new ClassSimpleServiceModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .OrderBy(c => c.Name)
                .ToList();

        public IEnumerable<GameServiceSimpleModel> GetGames()
        => this.dbContext.Games
            .Select(g => new GameServiceSimpleModel
            {
                Id = g.Id,
                Name = g.Name
            })
            .OrderBy(c => c.Name)
            .ToList();
    }
}
