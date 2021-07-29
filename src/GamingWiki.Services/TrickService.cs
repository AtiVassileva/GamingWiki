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
            => this.GetTricks(this.dbContext.Tricks)
                .OrderByDescending(t => t.Id);

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

        public bool TrickExists(int trickId)
            => this.dbContext.Tricks.Any(t => t.Id == trickId);

        public void Edit(int trickId, string heading, string content, string pictureUrl)
        {
            var trick = this.FindTrick(trickId);

            if (trick == null)
            {
                return;
            }

            trick.Heading = heading;
            trick.Content = content;
            trick.PictureUrl = pictureUrl;

            this.dbContext.SaveChanges();
        }

        public void Delete(int trickId)
        {
            var trick = this.FindTrick(trickId);

            if (trick == null)
            {
                return;
            }

            this.dbContext.Tricks.Remove(trick);
            this.dbContext.SaveChanges();
        }

        public IEnumerable<TrickServiceListingModel> Search(string searchCriteria)
            => this.GetTricks(this.dbContext.Tricks.Where(t =>
                t.Heading.ToLower().Contains(searchCriteria.ToLower()) 
                || t.Content.ToLower().Contains(searchCriteria.ToLower())));

        public string GetTrickAuthorId(int trickId)
            => this.dbContext.Tricks
                .First(t => t.Id == trickId).AuthorId;

        public TrickServiceListingModel Details(int trickId)
            => this.GetTricks(this.dbContext.Tricks
                .Where(t => t.Id == trickId)).FirstOrDefault();

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

        private Trick FindTrick(int trickId) 
            => this.dbContext.Tricks
                .FirstOrDefault(t => t.Id == trickId);

        private IEnumerable<TrickServiceListingModel> GetTricks(IQueryable<Trick> tricksQuery) 
            => tricksQuery
                .Select(t => new TrickServiceListingModel
                {
                    Id = t.Id,
                    Heading = t.Heading,
                    Content = t.Content,
                    PictureUrl = t.PictureUrl,
                    GameId = t.GameId,
                    Game = t.Game.Name,
                    Author = t.Author.UserName,
                    AuthorId = t.AuthorId
                }).ToList();
    }
}
