using System;
using System.Collections.Generic;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Services.Models.Games;
using GamingWiki.Services.Models.Tricks;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Home;
using MyTested.AspNetCore.Mvc;
using Xunit;
using static GamingWiki.Web.Common.WebConstants.Cache;

namespace GamingWiki.Tests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void IndexShouldReturnGuestPageViewForUnauthenticatedUser()
            => MyController<HomeController>
                .Instance()
                .Calling(c => c.Index())
                .ShouldReturn()
                .View("GuestPage");

        [Fact]
        public void IndexShouldReturnIndexViewForAuthenticatedUser()
        => MyController<HomeController>
            .Instance(controller => controller
                .WithUser(TestUser.Identifier))
            .Calling(c => c.Index())
            .ShouldHave()
            .MemoryCache(cache => cache
                .ContainingEntry(entry => entry
                    .WithKey(LatestArticlesCacheKey)
                    .WithAbsoluteExpirationRelativeToNow(TimeSpan
                        .FromMinutes(CacheExpiringTime))
                    .WithValueOfType<List<ArticleServiceHomeModel>>())
                .ContainingEntry(entry => entry
                    .WithKey(LatestGamesCacheKey)
                    .WithAbsoluteExpirationRelativeToNow(TimeSpan
                        .FromMinutes(CacheExpiringTime))
                    .WithValueOfType<List<GameServiceListingModel>>())
                .ContainingEntry(entry => entry
                    .WithKey(LatestTricksCacheKey)
                    .WithAbsoluteExpirationRelativeToNow(TimeSpan
                        .FromMinutes(CacheExpiringTime))
                    .WithValueOfType<List<TrickServiceHomeModel>>()))
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<HomeViewModel>());

        [Fact]
        public void AboutShouldReturnView()
        => MyController<HomeController>
            .Instance()
            .Calling(c => c.About())
            .ShouldReturn()
            .View();

        [Fact]
        public void ErrorShouldReturnIndexView()
            => MyController<HomeController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Error())
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>());
    }
}
