using GamingWiki.Services.Models.Games;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models.Games;
using static GamingWiki.Tests.Data.Games;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace GamingWiki.Tests.Routing
{
    public class GamesRouteTests
    {
        [Fact]
        public void AllShouldBeMappedWithNoPageParameter()
            => MyRouting
                .Configuration()
                .ShouldMap("/Games/All")
                .To<GamesController>(c =>
                    c.All(With.No<int>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameFullModel>());

        [Fact]
        public void AllShouldBeMappedWithPageParameter()
            => MyRouting
                .Configuration()
                .ShouldMap("/Games/All?pageIndex=1")
                .To<GamesController>(c =>
                    c.All(1))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameFullModel>());

        [Fact]
        public void GetCreateShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Games/Create")
                .To<GamesController>(c => c.Create())
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameFormModel>());

        [Fact]
        public void PostCreateShouldBeMapped()
        => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithMethod(HttpMethod.Post)
                .WithLocation("/Games/Create"))
            .To<GamesController>(c =>
                c.Create(With.Any<GameFormModel>()));

        [Fact]
        public void DetailsShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap($"/Games/Details?gameId={TestGame.Id}")
                .To<GamesController>(c =>
                    c.Details(TestGame.Id))
                .Which(controller => controller
                    .WithData(TestGame))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameServiceDetailsModel>());

        [Fact]
        public void GetEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap($"/Games/Edit?gameId={TestGame.Id}")
                .To<GamesController>(c =>
                    c.Edit(TestGame.Id))
            .Which(controller => controller
                    .WithData(TestGame)
                    .WithUser(user => user.InRole(AdministratorRoleName)))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameServiceEditModel>());

        [Fact]
        public void PostEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"/Games/Edit?gameId={TestGame.Id}"))
                .To<GamesController>(c =>
                    c.Edit(With.Any<GameServiceEditModel>(),
                        TestGame.Id));

        [Fact]
        public void DeleteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Games/Delete?gameId=1")
                .To<GamesController>(c =>
                    c.Delete(1));

        [Fact]
        public void SearchShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Games/Search?parameter=a")
                .To<GamesController>(c =>
                    c.Search("a", With.No<int>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameFullModel>());

        [Fact]
        public void SearchShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap("/Games/Search?parameter=a&pageIndex=1")
            .To<GamesController>(c =>
                c.Search("a", 1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<GameFullModel>());

        [Fact]
        public void FilterShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Games/Filter?parameter=1")
                .To<GamesController>(c =>
                    c.Filter(1, With.No<int>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameFullModel>());

        [Fact]
        public void FilterShouldBeMappedWithPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Games/Filter?parameter=1&pageIndex=1")
                .To<GamesController>(c =>
                    c.Filter(1, 1))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameFullModel>());


        [Fact]
        public void MineShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Games/Mine")
                .To<GamesController>(c =>
                    c.Mine(With.No<int>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameFullModel>());

        [Fact]
        public void MineShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap("/Games/Mine?pageIndex=1")
            .To<GamesController>(c =>
                c.Mine(1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<GameFullModel>());
    }
}
