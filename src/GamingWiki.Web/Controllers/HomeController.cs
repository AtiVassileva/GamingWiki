using System;
using System.Collections.Generic;
using GamingWiki.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Services.Models.Games;
using GamingWiki.Services.Models.Tricks;
using GamingWiki.Web.Models.Home;
using static GamingWiki.Web.Common.WebConstants.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace GamingWiki.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGameService gameService;
        private readonly IArticleService articleService;
        private readonly ITrickService trickService;
        private readonly IMemoryCache cache;

        public HomeController(IGameService gameService, IArticleService articleService, ITrickService trickService, IMemoryCache cache)
        {
            this.articleService = articleService;
            this.trickService = trickService;
            this.cache = cache;
            this.gameService = gameService;
        }

        public IActionResult Index()
        {
            var userIdentity = this.User.Identity;

            if (userIdentity is { IsAuthenticated: false })
            {
                return this.View("GuestPage");
            }

            var latestArticles = this.cache
                .Get<IEnumerable<ArticleServiceHomeModel>>(LatestArticlesCacheKey) 
                                 ?? this.SetLatestArticles();

            var latestGames = this.cache
                .Get<IEnumerable<GameServiceListingModel>>(LatestGamesCacheKey) 
                              ?? this.SetLatestGames();

            var latestTricks = this.cache
                .Get<IEnumerable<TrickServiceHomeModel>>(LatestTricksCacheKey) 
                               ?? this.SetLatestTricks();

            return this.View(new HomeViewModel
            {
                LatestArticles = latestArticles,
                LatestGames = latestGames,
                LatestTricks = latestTricks
            });
        }

        private IEnumerable<ArticleServiceHomeModel> SetLatestArticles()
        {
            var latestArticles = this.articleService.GetLatest().ToList();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheExpiringTime));

            this.cache.Set(LatestArticlesCacheKey, latestArticles, cacheOptions);

            return latestArticles;
        }

        private IEnumerable<GameServiceListingModel> SetLatestGames()
        {
            var latestGames = this.gameService.GetLatest().ToList();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheExpiringTime));

            this.cache.Set(LatestGamesCacheKey, latestGames, cacheOptions);

            return latestGames;
        }

        private IEnumerable<TrickServiceHomeModel> SetLatestTricks()
        {
            var latestTricks = this.trickService.GetLatest().ToList();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheExpiringTime));

            this.cache.Set(LatestTricksCacheKey, latestTricks, cacheOptions);

            return latestTricks;
        }

        public IActionResult About() => this.View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
