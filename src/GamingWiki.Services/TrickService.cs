using System.Collections.Generic;
using System.Linq;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Games;
using GamingWiki.Services.Models.Tricks;

namespace GamingWiki.Services
{
    public class TrickService : ITrickService
    {
        private const int HomePageEntityCount = 3;

        private readonly ApplicationDbContext dbContext;

        public TrickService(ApplicationDbContext dbContext)
            => this.dbContext = dbContext;

        public IEnumerable<TrickServiceListingModel> All()
            => this.dbContext.Tricks
                .Select(t => new TrickServiceListingModel
                {
                    Id = t.Id,
                    Heading = t.Heading,
                    Content = t.Content,
                    PictureUrl = t.PictureUrl,
                    GameId = t.GameId,
                    Game = t.Game.Name,
                    Author = t.Author.UserName
                })
                .OrderByDescending(t => t.Id)
                .ToList();

        public void Create(string heading, string content, string authorId, string pictureUrl, int gameId)
        {
            var trick = new Trick
            {
                Heading = heading,
                Content = content,
                AuthorId = authorId,
                PictureUrl = pictureUrl,
                GameId = gameId
            };

            this.dbContext.Tricks.Add(trick);
            this.dbContext.SaveChanges();
        }

        public bool GameExists(int gameId)
            => this.dbContext.Games.Any(g => g.Id == gameId);

        public IEnumerable<GameServiceSimpleModel> GetGames()
            => this.dbContext.Games
                .Select(g => new GameServiceSimpleModel
                {
                    Id = g.Id,
                    Name = g.Name
                }).OrderBy(g => g.Name)
                .ToList();

        public IEnumerable<TrickServiceHomeModel> GetLatest()
            => this.dbContext.Tricks
                .Select(t => new TrickServiceHomeModel
                {
                    Id = t.Id,
                    Heading = t.Heading,
                    Content = t.Content,
                    GameId = t.GameId,
                    Game = t.Game.Name
                }).OrderByDescending(t => t.Id)
                .Take(HomePageEntityCount)
                .ToList();
    }
}
