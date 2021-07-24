using System;
using System.Collections.Generic;
using System.Linq;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Services.Contracts;

namespace GamingWiki.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext dbContext;

        public GameService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Creator> ParseCreators(string creatorsNames)
        {
            var creators = new List<Creator>();

            foreach (var creatorName in creatorsNames
                .Split(", "))
            {
                var creator = this.dbContext.Creators
                    .FirstOrDefault
                        (c => c.Name == creatorName);

                if (creator == null)
                {
                    creator = new Creator
                    {
                        Name = creatorName
                    };

                    this.dbContext.Creators.Add(creator);
                    this.dbContext.SaveChanges();
                }

                creators.Add(creator);
            }

            return creators;
        }

        public IDictionary<string, double> GetRatings(int gameId)
        {
            var reviews = this.dbContext.Reviews
                .Where(r => r.GameId == gameId).ToList();

            var ratings = new Dictionary<string, double>()
            {
                { "Price" ,  reviews.Count > 0 ?reviews.Select(r => r.PriceRate).Average() : 0.0},
                { "Graphics" , reviews.Count > 0 ? reviews.Select(r => r.GraphicsRate)
                    .Average() : 0.0},
                { "Levels" , reviews.Count > 0 ? reviews.Select(r => r.LevelsRate).Average() : 0.0},
                { "Difficulty" , reviews.Count > 0 ? reviews.Select(
                    r => r.DifficultyRate).Average() : 0.0},
            };

            return ratings;
        }

        public bool AreaExists(int areaId)
            => this.dbContext.Areas.Any(a => a.Id == areaId);

        public bool GenreExists(int genreId)
            => this.dbContext.Genres.Any(g => g.Id == genreId);

        
        public int Create(string name, string pictureUrl, string trailerUrl, string description, int areaId, int genreId, string creatorsNames)
        {
            var game = new Game
            {
                Name = name,
                PictureUrl = pictureUrl,
                TrailerUrl = trailerUrl,
                Description = description,
                AreaId = areaId,
                GenreId = genreId
            };

            this.dbContext.Games.Add(game);
            this.dbContext.SaveChanges();

            var creators = this.ParseCreators(creatorsNames);

            this.dbContext.GamesCreators.AddRange(this.ParseGamesCreators(creators, game.Id));
            this.dbContext.SaveChanges();

            return game.Id;
        }

        private IEnumerable<GameCreator> ParseGamesCreators(IEnumerable<Creator> creators, int gameId) 
            => creators.Select(c => new GameCreator
            {
                GameId = gameId,
                CreatorId = c.Id
            });

        //public IEnumerable<CharacterGameModel> GetCharacters(int gameId)
        //    => this.dbContext.Characters
        //        .Where(c => c.GameId == gameId)
        //        .Select(c => new CharacterGameModel
        //        {
        //            Id = c.Id,
        //            Name = c.Name
        //        })
        //        .OrderBy(c => c.Name)
        //        .ToList();
    }
}
