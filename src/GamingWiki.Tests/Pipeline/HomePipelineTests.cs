using System.Linq;
using GamingWiki.Web.Controllers;
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
                .WithModelOfType<HomeViewModel>()
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
                .View();
    }
}
