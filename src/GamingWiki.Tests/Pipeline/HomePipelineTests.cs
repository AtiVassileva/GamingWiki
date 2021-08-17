using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models.Home;
using MyTested.AspNetCore.Mvc;
using static GamingWiki.Tests.Data.Games;
using static GamingWiki.Tests.Data.Articles;
using static GamingWiki.Tests.Data.Tricks;
using Xunit;

namespace GamingWiki.Tests.Pipeline
{
    public class HomePipelineTests
    {
        [Fact]
        public void IndexShouldReturnCorrectViewWithModel()
        => MyRouting
            .Configuration()
            .ShouldMap("/")
            .To<HomeController>(c => c.Index())
            .Which(controller => controller
                .WithData(ArticleTestData))
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<HomeViewModel>()
                .Passing(articles => articles.Count.ShouldBe(expected)));
    }
}
