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

        public bool GameExists(string gameName)
            => this.dbContext.Games.Any(g => g.Name == gameName);

        public string GetContributorId(int gameId)
            => this.dbContext.Games
                .Where(g => g.Id == gameId)
                .Select(g => g.ContributorId)
                .First();

        public int Create(string name, string pictureUrl, string trailerUrl, string description, int areaId, int genreId, string creatorsNames, string contributorId,
            bool isApproved,
            IEnumerable<int> supportedPlatforms)
        {
            var game = new Game
            {
                Name = name,
                PictureUrl = pictureUrl,
                TrailerUrl = trailerUrl,
                Description = description,
                AreaId = areaId,
                GenreId = genreId,
                ContributorId = contributorId,
                IsApproved = isApproved
            };

            this.dbContext.Games.Add(game);
            this.dbContext.SaveChanges();

            var creators = this.ParseCreators(creatorsNames);

            this.dbContext.GamesCreators.AddRange(ParseGamesCreators(creators, game.Id));

            var gamePlatforms = ParsePlatforms(game.Id, supportedPlatforms);

            this.dbContext.GamesPlatforms.AddRange(gamePlatforms);

            this.dbContext.SaveChanges();

            return game.Id;
        }

        public IQueryable<GameServiceListingModel> All(bool approvedOnly = true)
        {
            var gamesQuery = this.dbContext.Games.AsQueryable();

            if (approvedOnly)
            {
                gamesQuery = gamesQuery.Where(g => g.IsApproved);
            }

            return gamesQuery
                .ProjectTo<GameServiceListingModel>(this.configuration)
                .OrderBy(g => g.Name);
        }

        public GameServiceDetailsModel Details(int gameId)
        {
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

        public bool Edit(int gameId, GameServiceEditModel model)
        {
            if (!this.GameExists(gameId))
            {
                return false;
            }

            var game = this.FindGame(gameId);

            game.Description = model.Description;
            game.PictureUrl = model.PictureUrl;
            game.AreaId = model.AreaId;
            game.TrailerUrl = model.TrailerUrl;
            game.IsApproved = model.IsApproved;

            var oldPlatforms = this.GetGamePlatforms(game.Id).ToList();
            var newPlatforms = model.SupportedPlatforms.ToList();

            if ((!oldPlatforms.All(newPlatforms.Contains) 
                || !newPlatforms.All(oldPlatforms.Contains)) 
                && oldPlatforms.Count != newPlatforms.Count)
            {
                this.ParseEditedPlatforms(oldPlatforms, newPlatforms, game);
            }

            this.dbContext.SaveChanges();
            return true;
        }

        public bool Delete(int gameId)
        {
            if (!this.GameExists(gameId))
            {
                return false;
            }

            var game = this.FindGame(gameId);

            this.dbContext.Games.Remove(game);
            this.dbContext.SaveChanges();

            return true;
        }

        public void Approve(int gameId)
        {
            var game = this.FindGame(gameId);
            game.IsApproved = true;
            this.dbContext.SaveChanges();
        }

        public IQueryable<GameServiceListingModel> Mine(string contributorId)
            => this.dbContext.Games
                .Where(g => g.ContributorId == contributorId)
                .ProjectTo<GameServiceListingModel>(this.configuration)
                .OrderByDescending(g => g.Id);

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

        public IEnumerable<GamePendingModel> GetPending()
            => this.dbContext.Games
                .Where(g => !g.IsApproved)
                .ProjectTo<GamePendingModel>(this.configuration)
                .OrderByDescending(g => g.Id)
                .ToList();

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

        public IEnumerable<ReviewServiceSimpleModel> GetReviews(int gameId)
            => this.dbContext.Reviews
                .Where(r => r.GameId == gameId)
                .ProjectTo<ReviewServiceSimpleModel>(this.configuration)
                .OrderByDescending(r => r.Id)
                .ToList();

        public IEnumerable<PlatformServiceModel> GetPlatforms()
            => this.dbContext.Platforms
                .ProjectTo<PlatformServiceModel>(this.configuration)
                .OrderBy(p => p.Name.Length)
                .ToList();

        public IEnumerable<int> GetGamePlatforms(int gameId)
            => this.dbContext.GamesPlatforms
                .Where(gp => gp.GameId == gameId)
                .Select(gp => gp.PlatformId)
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

        public IEnumerable<string> GetPlatformsByGame(int gameId)
            => this.dbContext.GamesPlatforms
                .Where(gp => gp.GameId == gameId)
                .Select(gp => gp.Platform.Name)
                .OrderBy(p => p.Length)
                .ToList();

        private static IEnumerable<GamePlatform> ParsePlatforms(int gameId, IEnumerable<int> supportedPlatforms)
            => supportedPlatforms
                .Select(platformId => new GamePlatform
                {
                    GameId = gameId,
                    PlatformId = platformId
                });

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

        private void ParseEditedPlatforms(IEnumerable<int> oldPlatformsList, IEnumerable<int> newPlatformsList, Game game)
        {

            var oldPlatforms = oldPlatformsList.ToList();
            var newPlatforms = newPlatformsList.ToList();

            this.RemoveOldPlatforms(game, oldPlatforms, newPlatforms);
            this.AddNewPlatforms(game, newPlatforms, oldPlatforms);
        }

        private void RemoveOldPlatforms(Game game, IList<int> oldPlatforms, IList<int> newPlatforms)
        {
            foreach (var platformId in oldPlatforms)
            {
                if (!newPlatforms.Contains(platformId))
                {
                    var entity = this.dbContext.GamesPlatforms
                        .First(gp => gp.GameId == game.Id
                                     && gp.PlatformId == platformId);

                    this.dbContext.GamesPlatforms.Remove(entity);
                }
            }
        }
        private void AddNewPlatforms(Game game, IList<int> newPlatforms, IList<int> oldPlatforms)
        {
            foreach (var platformId in newPlatforms)
            {
                if (!oldPlatforms.Contains(platformId))
                {
                    var entity = new GamePlatform
                    {
                        GameId = game.Id,
                        PlatformId = platformId
                    };

                    this.dbContext.GamesPlatforms.Add(entity);
                }
            }
        }
    }
}
