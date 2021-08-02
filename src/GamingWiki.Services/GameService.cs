using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Areas;
using GamingWiki.Services.Models.Characters;
using GamingWiki.Services.Models.Games;
using GamingWiki.Services.Models.Genres;
using GamingWiki.Services.Models.Platforms;
using GamingWiki.Services.Models.Reviews;
using Microsoft.EntityFrameworkCore;
using static GamingWiki.Services.Common.ServiceConstants;
using static GamingWiki.Services.Common.ExceptionMessages;

namespace GamingWiki.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IConfigurationProvider configuration;

        public GameService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.configuration = mapper.ConfigurationProvider;
        }

        public IEnumerable<Creator> ParseCreators(string creatorsNames)
        {
            foreach (var creatorName in creatorsNames
                .Split(", "))
            {
                var creator = this.dbContext.Creators
                    .FirstOrDefault(c => c.Name == creatorName);

                if (creator == null)
                {
                    creator = new Creator
                    {
                        Name = creatorName
                    };

                    this.dbContext.Creators.Add(creator);
                    this.dbContext.SaveChanges();
                }

                yield return creator;
            }
            
        }

        public bool AreaExists(int areaId)
            => this.dbContext.Areas.Any(a => a.Id == areaId);

        public bool GenreExists(int genreId)
            => this.dbContext.Genres.Any(g => g.Id == genreId);

        public bool GameExists(int gameId)
            => this.dbContext.Games.Any(g => g.Id == gameId);


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

        public IQueryable<GameServiceListingModel> All()
            => this.dbContext.Games
                .ProjectTo<GameServiceListingModel>(this.configuration)
                .OrderBy(g => g.Name);

        public GameServiceDetailsModel Details(int gameId)
        {
            if (!this.GameExists(gameId))
            {
                throw new InvalidOperationException(NonExistingGameExceptionMessage);
            }

            var game = this.FindGame(gameId);

            var gameDetails = this.mapper
                .Map<GameServiceDetailsModel>(game);

            gameDetails.Ratings = this.GetRatings(gameDetails.Id);
            gameDetails.Rating = gameDetails.Ratings.Values.Average();
            gameDetails.Creators = this.GetCreators(gameDetails.Id);
            gameDetails.Characters = this.GetCharacters(gameDetails.Id);
            gameDetails.Reviews = this.GetReviews(gameDetails.Id);
            gameDetails.Platforms = this.GetPlatformsByGame(gameDetails.Id);

            return gameDetails;
        }

        public void Edit(int gameId, string description, string pictureUrl, int areaId, string trailerUrl)
        {
            if (!this.GameExists(gameId))
            {
                throw new InvalidOperationException(NonExistingGameExceptionMessage);
            }

            var game = this.FindGame(gameId);

            game.Description = description;
            game.PictureUrl = pictureUrl;
            game.AreaId = areaId;
            game.TrailerUrl = trailerUrl;

            this.dbContext.SaveChanges();
        }

        public void Delete(int gameId)
        {
            if (!this.GameExists(gameId))
            {
                throw new InvalidOperationException(NonExistingGameExceptionMessage);
            }

            var game = this.FindGame(gameId);

            this.dbContext.Games.Remove(game);
            this.dbContext.SaveChanges();
        }

        public IQueryable<GameServiceListingModel> Search(string letter) 
            => this.dbContext.Games
                .Where(g => g.Name.ToUpper()
                    .StartsWith(letter))
                .ProjectTo<GameServiceListingModel>(this.configuration)
                .OrderBy(g => g.Name);

        public IQueryable<GameServiceListingModel> Filter(int genreId)
            => this.dbContext.Games
                .Where(g => g.GenreId == genreId)
                .ProjectTo<GameServiceListingModel>(this.configuration)
                .OrderBy(g => g.Name);

        public IEnumerable<AreaServiceModel> GetAreas()
            => this.dbContext
                .Areas
                .ProjectTo<AreaServiceModel>(this.configuration)
                .OrderBy(a => a.Name)
                .ToList();


        public IEnumerable<GenreServiceModel> GetGenres()
            => this.dbContext.Genres
                .ProjectTo<GenreServiceModel>(this.configuration)
                .OrderBy(g => g.Name)
                .ToList();

        public IEnumerable<ReviewDetailsServiceModel> GetReviews(int gameId)
            => this.dbContext.Reviews
                .Where(r => r.GameId == gameId)
                .ProjectTo<ReviewDetailsServiceModel>(this.configuration)
                .OrderByDescending(r => r.Id)
                .ToList();

        public IEnumerable<PlatformServiceModel> GetPlatforms()
            => this.dbContext.Platforms
                .ProjectTo<PlatformServiceModel>(this.configuration)
                .OrderBy(p => p.Name.Length)
                .ToList();

        public IEnumerable<GameServiceListingModel> GetLatest() 
            => this.dbContext.Games
                .ProjectTo<GameServiceListingModel>(this.configuration)
                .OrderByDescending(g => g.Id)
                .Take(HomePageEntityCount)
                .ToList();

        public IDictionary<string, double> GetRatings(int gameId)
        {
            var reviews = this.dbContext.Reviews
                .Where(r => r.GameId == gameId).ToList();

            var ratings = new Dictionary<string, double>
            {
                { "Price" ,  reviews.Count > 0 ? reviews.Select(r => r.PriceRate).Average() : 0.0},
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

        private Game FindGame(int gameId)
            => this.dbContext.Games
                .Include(g => g.Area)
                .Include(g => g.Genre)
                .First(g => g.Id == gameId);

        private IEnumerable<string> GetCreators(int gameId)
            => this.dbContext.GamesCreators
                .Where(gc => gc.GameId == gameId)
                .Select(gc => gc.Creator.Name).ToList();

        private IEnumerable<CharacterGameServiceModel> GetCharacters(int gameId)
            => this.dbContext.Characters
                .Where(c => c.GameId == gameId)
                .ProjectTo<CharacterGameServiceModel>(this.configuration)
                .OrderBy(c => c.Name)
                .ToList();

        public IEnumerable<string> GetPlatformsByGame(int gameId)
            => this.dbContext.GamesPlatforms
                .Where(gp => gp.GameId == gameId)
                .Select(gp => gp.Platform.Name)
                .OrderBy(p => p.Length)
                .ToList();
    }
}
