using GamingWiki.Services.Models.Games;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Games;
using static GamingWiki.Tests.Data.Games;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using MyTested.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace GamingWiki.Tests.Pipeline
{
    public class GamesPipelineTests
    {
        [Fact]
        public void AllWithoutPageShouldReturnCorrectViewWithCorrectModel()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Games/All")
                .WithUser()
                .WithAntiForgeryToken())
            .To<GamesController>(c => c.All(With.No<int>()))
            .Which(controller => controller
                .WithData(FiveGames))
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<GameFullModel>()
                .Passing(gameListing =>
                {
                    gameListing.Games.Count.ShouldBe(3);
                    gameListing.Games.PageIndex.ShouldBe(0);
                    gameListing.Games.TotalPages.ShouldBe(2);
                }));

        [Theory]
        [InlineData(1, 3, 2)]
        [InlineData(2, 2, 2)]
        public void AllWithPageShouldReturnCorrectViewWithCorrectModel(int pageIndex, int expectedCountOnPage, int totalPages)
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Games/All?pageIndex={pageIndex}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<GamesController>(c => c.All(pageIndex))
                .Which(controller => controller
                    .WithData(FiveGames))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameFullModel>()
                    .Passing(gameListing =>
                    {
                        gameListing.Games.Count.ShouldBe(expectedCountOnPage);
                        gameListing.Games.PageIndex.ShouldBe(pageIndex);
                        gameListing.Games.TotalPages.ShouldBe(totalPages);
                    }));

        [Fact]
        public void GetCreateShouldBeMappedAndReturnCorrectView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation("/Games/Create")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<GamesController>(c => c.Create())
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameFormModel>());

        [Fact]
        public void PostCreateShouldBeMappedAndHaveInvalidModelStateAndReturnView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/Games/Create")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<GamesController>(c => c.Create(new GameFormModel()))
                .Which()
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameFormModel>());

        [Fact]
        public void DetailsShouldBeMappedAndReturnCorrectViewWithValidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Games/Details?gameId={TestGame.Id}")
                .WithUser()
                .WithAntiForgeryToken())
            .To<GamesController>(c => c.Details(TestGame.Id))
            .Which(controller => controller
                .WithData(TestGame))
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<GameServiceDetailsModel>());

        [Fact]
        public void DetailsShouldBeMappedAndReturnErrorViewWithInvalidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Games/Details?gameId={TestGame.Id}")
                .WithUser()
                .WithAntiForgeryToken())
            .To<GamesController>(c => c.Details(TestGame.Id))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ErrorViewModel>());

        [Fact]
        public void GetEditShouldBeMappedAndReturnCorrectViewWithValidId()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Games/Edit?gameId={TestGame.Id}")
                    .WithUser(user => user.InRole(AdministratorRoleName))
                    .WithAntiForgeryToken())
                .To<GamesController>(c => c.Edit(TestGame.Id))
                .Which(controller => controller
                    .WithData(TestGame))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameServiceEditModel>());

        [Fact]
        public void GetEditShouldBeMappedAndReturnUnauthorizedForUnauthorizedUsers()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Games/Edit?gameId={TestGame.Id}")
                    .WithUser(TestUser.Identifier)
                    .WithAntiForgeryToken())
                .To<GamesController>(c => c.Edit(TestGame.Id))
                .Which(controller => controller
                    .WithData(TestGame))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void GetEditShouldBeMappedAndReturnErrorViewWithInvalidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Games/Edit?gameId={TestGame.Id}")
                .WithUser(TestUser.Identifier)
                .WithAntiForgeryToken())
            .To<GamesController>(c => c.Edit(TestGame.Id))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ErrorViewModel>());

        [Fact]
        public void PostEditShouldBeMappedAndHaveInvalidModelStateAndReturnView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"/Games/Edit?gameId={TestGame.Id}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<GamesController>(c => c.Edit(new GameServiceEditModel(), TestGame.Id))
                .Which(controller => controller
                    .WithData(TestGame))
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameServiceEditModel>());
        [Fact]
        public void DeleteShouldBeMappedAndRedirectUponSuccessfulAction()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Games/Delete?gameId={TestGame.Id}")
                    .WithUser(user => user.InRole(AdministratorRoleName))
                    .WithAntiForgeryToken())
                .To<GamesController>(c => c.Delete(TestGame.Id))
                .Which(controller => controller
                    .WithData(TestGame))
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<GamesController>(c => c
                        .All(With.Any<int>())));

        [Fact]
        public void DeleteShouldBeMappedAndReturnUnauthorizedForUnauthorizedUsers()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Games/Delete?gameId={TestGame.Id}")
                .WithUser(TestUser.Identifier)
                .WithAntiForgeryToken())
            .To<GamesController>(c => c.Delete(TestGame.Id))
            .Which(controller => controller
                .WithData(TestGame))
            .ShouldReturn()
            .Unauthorized();

        [Fact]
        public void DeleteShouldBeMappedAndReturnErrorViewWithInvalidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Games/Delete?gameId={TestGame.Id}")
                .WithUser(user => user.InRole(AdministratorRoleName))
                .WithAntiForgeryToken())
            .To<GamesController>(c =>
                c.Delete(TestGame.Id))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ErrorViewModel>());

        [Fact]
        public void SearchShouldBeMappedAndReturnCorrectViewWithPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Games/Search?parameter=a&pageIndex=1")
                .WithUser()
                .WithAntiForgeryToken())
            .To<GamesController>(c =>
                c.Search("a", 1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<GameFullModel>());

        [Fact]
        public void SearchShouldBeMappedAndReturnCorrectViewWithoutPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
            .WithLocation("/Games/Search?parameter=a")
            .WithUser()
            .WithAntiForgeryToken())
            .To<GamesController>(c =>
        c.Search("a", With.No<int>()))
            .Which()
            .ShouldReturn()
            .View(view => view
            .WithModelOfType<GameFullModel>());

        [Fact]
        public void FilterShouldBeMappedAndReturnCorrectViewWithoutPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Games/Filter?parameter=1")
                .WithUser()
                .WithAntiForgeryToken())
            .To<GamesController>(c =>
                c.Filter(1, With.No<int>()))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<GameFullModel>());

        [Fact]
        public void FilterShouldBeMappedAndReturnCorrectViewWithPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Games/Filter?parameter=1&pageIndex=1")
                .WithUser()
                .WithAntiForgeryToken())
            .To<GamesController>(c =>
                c.Filter(1, 1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<GameFullModel>());

        [Fact]
        public void MineShouldBeMappedAndReturnCorrectViewWithPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Games/Mine?pageIndex=1")
                .WithUser()
                .WithAntiForgeryToken())
            .To<GamesController>(c =>
                c.Mine(1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<GameFullModel>());

        [Fact]
        public void MineShouldBeMappedAndReturnCorrectViewWithoutPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Games/Mine")
                .WithUser()
                .WithAntiForgeryToken())
            .To<GamesController>(c =>
                c.Mine(With.No<int>()))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<GameFullModel>());
    }
}
