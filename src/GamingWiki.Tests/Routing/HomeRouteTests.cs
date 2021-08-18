using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Home;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace GamingWiki.Tests.Routing
{
    public class HomeRouteTests
    {
        [Fact]
        public void IndexShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/")
                .To<HomeController>(c => c.Index())
                .Which(controller => controller
                    .WithUser())
                .ShouldReturn()
                .View(view => view.WithModelOfType<HomeViewModel>());

        [Fact]
        public void AboutShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Home/About")
                .To<HomeController>(c => c.About())
                .Which()
                .ShouldReturn()
                .View();

        [Fact]
        public void ErrorShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Home/Error")
                .To<HomeController>(c => c.Error())
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>());
    }
}
