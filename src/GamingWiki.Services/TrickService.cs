using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Games;
using GamingWiki.Services.Models.Tricks;
using static GamingWiki.Services.Common.ServiceConstants;
using static GamingWiki.Services.Common.ExceptionMessages;

namespace GamingWiki.Services
{
    public class TrickService : ITrickService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfigurationProvider configuration;

        public TrickService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.configuration = mapper.ConfigurationProvider;
        }

        public IQueryable<TrickServiceListingModel> All()
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
            if (!this.TrickExists(trickId))
            {
                throw new InvalidOperationException(NonExistingTrickExceptionMessage);
            }

            var trick = this.FindTrick(trickId);

            trick.Heading = heading;
            trick.Content = content;
            trick.PictureUrl = pictureUrl;

            this.dbContext.SaveChanges();
        }

        public void Delete(int trickId)
        {
            if (!this.TrickExists(trickId))
            {
                throw new InvalidOperationException(NonExistingTrickExceptionMessage);
            }

            var trick = this.FindTrick(trickId);

            this.dbContext.Tricks.Remove(trick);
            this.dbContext.SaveChanges();
        }

        public IQueryable<TrickServiceListingModel> Search(string searchCriteria)
            => this.GetTricks(this.dbContext.Tricks.Where(t =>
                t.Heading.ToLower()
                    .Contains(searchCriteria.ToLower()) || t.Content.ToLower()
                    .Contains(searchCriteria.ToLower()) ||
                t.Game.Name.ToLower()
                    .Contains(searchCriteria.ToLower())));

        public string GetTrickAuthorId(int trickId)
            => this.dbContext.Tricks
                .First(t => t.Id == trickId).AuthorId;

        public TrickServiceListingModel Details(int trickId)
            => this.GetTricks(this.dbContext.Tricks
                .Where(t => t.Id == trickId))
                .FirstOrDefault();

        public IEnumerable<GameServiceSimpleModel> GetGames()
            => this.dbContext.Games
                .ProjectTo<GameServiceSimpleModel>(this.configuration)
                .OrderBy(g => g.Name)
                .ToList();

        public IEnumerable<TrickServiceHomeModel> GetLatest()
            => this.dbContext.Tricks
                .ProjectTo<TrickServiceHomeModel>(this.configuration)
                .OrderByDescending(t => t.Id)
                .Take(HomePageEntityCount)
                .ToList();

        public IQueryable<TrickServiceListingModel> GetTricksByUser(string userId)
            => this.GetTricks(this.dbContext.Tricks
                .Where(t => t.AuthorId == userId));

        private Trick FindTrick(int trickId)
            => this.dbContext.Tricks
                .First(t => t.Id == trickId);

        private IQueryable<TrickServiceListingModel> GetTricks(IQueryable tricksQuery)
            => tricksQuery
                .ProjectTo<TrickServiceListingModel>(this.configuration)
                .OrderByDescending(t => t.Id);
    }
}
