using System.Collections.Generic;
using System.Linq;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Services.Models.Games;
using GamingWiki.Services.Models.Tricks;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Home;
using MyTested.AspNetCore.Mvc;
using Shouldly;
using static GamingWiki.Tests.Data.Games;
using static GamingWiki.Tests.Data.Articles;
using static GamingWiki.Tests.Data.Tricks;
using Xunit;

namespace GamingWiki.Tests.Pipeline
{
    public class HomePipelineTests
    {
        [Fact]
        public void IndexShouldBeMappedAndReturnCorrectViewWithModel()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/")
                .WithUser()
                .WithAntiForgeryToken())
            .To<HomeController>(c => c.Index())
            .Which(controller => controller
                .WithData(FiveArticles)
                .WithData(FiveGames)
                .WithData(FiveTricks))
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<HomeViewModel>(m =>
                {
                    m.LatestGames.ShouldBeOfType(typeof(List<GameServiceListingModel>));
                    m.LatestArticles.ShouldBeOfType(typeof(List<ArticleServiceHomeModel>));
                    m.LatestTricks.ShouldBeOfType(typeof(List<TrickServiceHomeModel>));
                })
                .Passing(model =>
                    {
                        model.LatestArticles.Count().ShouldBe(3);
                        model.LatestGames.Count().ShouldBe(3);
                        model.LatestTricks.Count().ShouldBe(3);
                    }));

        [Fact]
        public void AboutShouldBeMappedAndReturnCorrectView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation("/Home/About")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<HomeController>(c => c.About())
                .Which()
                .ShouldReturn()
                .View(); 
        
        [Fact]
        public void ErrorShouldBeMappedAndReturnCorrectView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation("/Home/Error")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<HomeController>(c => c.Error())
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>());
    }
}
