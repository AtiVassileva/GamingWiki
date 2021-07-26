using System.Collections.Generic;
using System.Linq;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Areas;
using GamingWiki.Services.Models.Characters;
using GamingWiki.Services.Models.Games;
using GamingWiki.Services.Models.Genres;

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

            this.dbContext.GamesCreators.AddRange(ParseGamesCreators(creators, game.Id));

            this.dbContext.SaveChanges();

            return game.Id;
        }

        public IEnumerable<GameServiceListingModel> All()
        => this.dbContext.Games
            .Select(g => new GameServiceListingModel
            {
                Id = g.Id,
                Name = g.Name,
                PictureUrl = g.PictureUrl,
            }).OrderBy(g => g.Name)
            .ToList();

        public GameServiceDetailsModel Details(int gameId) 
            => this.dbContext.Games
                .Where(g => g.Id == gameId)
                .Select(g => new GameServiceDetailsModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    PictureUrl = g.PictureUrl,
                    TrailerUrl = g.TrailerUrl,
                    Description = g.Description,
                    Area = g.Area.Name,
                    Genre = g.Genre.Name,
                    Ratings = this.GetRatings(gameId),
                    Rating = this.GetRatings(gameId).Values.Average(),
                    Creators = this.dbContext.GamesCreators
                        .Where(gc => gc.GameId == gameId)
                        .Select(gc => gc.Creator.Name).ToList(),
                    Characters = this.dbContext.Characters
                        .Where(c => c.GameId == gameId)
                        .Select(c => new CharacterGameServiceModel
                        {
                            Id = c.Id,
                            Name = c.Name
                        })
                        .OrderBy(c => c.Name)
                        .ToList()
                }).FirstOrDefault();

        public void Edit(int gameId, string description, string pictureUrl, int areaId, string trailerUrl)
        {
            var game = this.dbContext.Games
                .FirstOrDefault(g => g.Id == gameId);

            if (game == null)
            {
                return;
            }

            game.Description = description;
            game.PictureUrl = pictureUrl;
            game.AreaId = areaId;
            game.TrailerUrl = trailerUrl;

            this.dbContext.SaveChanges();
        }

        public void Delete(int gameId)
        {
            var game = this.dbContext.Games
                .FirstOrDefault(g => g.Id == gameId);

            if (game == null)
            {
                return;
            }

            this.dbContext.Games.Remove(game);
            this.dbContext.SaveChanges();
        }

        public IEnumerable<GameServiceListingModel> Search(string letter) 
            => this.dbContext.Games
                .Where(g => g.Name.ToUpper()
                    .StartsWith(letter))
                .Select(g => new GameServiceListingModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    PictureUrl = g.PictureUrl,
                }).ToList();

        public IEnumerable<GameServiceListingModel> Filter(int genreId)
            => this.dbContext.Games
                .Where(g => g.GenreId == genreId)
                .Select(g => new GameServiceListingModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    PictureUrl = g.PictureUrl
                }).ToList();

        public IEnumerable<AreaServiceModel> GetAreas()
            => this.dbContext
                .Areas
                .Select(a =>
                    new AreaServiceModel
                    {
                        Id = a.Id,
                        Name = a.Name
                    })
                .OrderBy(a => a.Name)
                .ToList();


        public IEnumerable<GenreServiceModel> GetGenres()
            => this.dbContext
                .Genres
                .Select(g =>
                    new GenreServiceModel
                    {
                        Id = g.Id,
                        Name = g.Name
                    })
                .OrderBy(g => g.Name)
                .ToList();

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

        private static IEnumerable<GameCreator> ParseGamesCreators(IEnumerable<Creator> creators, int gameId)
            => creators.Select(c => new GameCreator
            {
                GameId = gameId,
                CreatorId = c.Id
            });

        //private IEnumerable<string> GetCreators(int gameId)
        //    => this.dbContext.GamesCreators
        //        .Where(gc => gc.GameId == gameId)
        //        .Select(gc => gc.Creator.Name).ToList();

        //private IEnumerable<CharacterGameServiceModel> GetCharacters(int gameId)
        //    => this.dbContext.Characters
        //        .Where(c => c.GameId == gameId)
        //        .Select(c => new CharacterGameServiceModel
        //        {
        //            Id = c.Id,
        //            Name = c.Name
        //        })
        //        .OrderBy(c => c.Name)
        //        .ToList();
    }
}
