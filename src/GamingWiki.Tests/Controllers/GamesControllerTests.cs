using System;
using GamingWiki.Models;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models.Games;
using MyTested.AspNetCore.Mvc;
using static GamingWiki.Tests.Common.TestConstants;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Tests.Data.Games;
using static GamingWiki.Tests.Data.Areas;
using static GamingWiki.Tests.Data.Genres;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using Xunit;
using System.Linq;
using GamingWiki.Services.Models.Games;
using GamingWiki.Web.Models;

namespace GamingWiki.Tests.Controllers
{
    public class GamesControllerTests
    {
        [Fact]
        public void GamesControllerShouldBeForAuthorizedUsersOnly()
        => MyController<GamesController>
            .ShouldHave()
            .Attributes(attributes => attributes
                .RestrictingForAuthorizedRequests());

        [Fact]
        public void AllShouldReturnCorrectViewWithModel()
            => MyController<GamesController>
                .Instance(controller => controller
                        .WithData(FiveGames))
                .Calling(c => c.All(DefaultPageIndex))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameFullModel>());

        [Fact]
        public void GetCreateShouldReturnCorrectView()
            => MyController<GamesController>
                .Instance()
                .Calling(c => c.Create())
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameFormModel>());

        [Fact]
        public void PostCreateReturnRedirectWithValidModel()
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithData(TestArea)
                    .WithData(TestGenre)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Create(TestValidGameFormModel))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                    .WithSet<Game>(games =>
                        games.Any(g =>
                        g.Name == TestValidGameFormModel.Name && g.Description == TestValidGameFormModel.Description)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<GamesController>
                        (c => c.Details(With.Any<int>())));

        [Fact]
        public void PostCreateShouldReturnViewWithInvalidModelState()
            => MyController<GamesController>
                .Instance()
                .Calling(c => c.Create(TestInvalidGameFormModel))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameFormModel>());

        [Fact]
        public void PostCreateShouldReturnViewWithExistingGameName()
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithData(TestGame))
                .Calling(c => c.Create(TestValidGameFormModel))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameFormModel>());

        [Fact]
        public void DetailsShouldReturnErrorViewWithInvalidGameId()
            => MyController<GamesController>
                .Instance()
                .Calling(c => c.Details(TestGame.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>());

        [Fact]
        public void DetailsShouldReturnCorrectViewWithValidGameId()
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithData(TestGame))
                .Calling(c => c.Details(TestGame.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameServiceDetailsModel>());

        [Fact]
        public void GetEditShouldReturnErrorViewWithInvalidId()
            => MyController<GamesController>
                .Instance()
                .Calling(c => c.Edit(TestGame.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>());

        [Fact]
        public void GetEditShouldReturnCorrectViewWithValidId()
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithUser(user => user
                        .InRole(AdministratorRoleName))
                    .WithData(TestGame))
                .Calling(c => c.Edit(TestGame.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameServiceEditModel>());

        [Fact]
        public void GetEditShouldReturnUnauthorizedForUnauthorizedUsers()
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithData(TestGame)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Edit(TestGame.Id))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void PostEditShouldReturnRedirectWithValidModel()
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithData(TestGame)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Edit(TestValidGameEditModel, TestGame.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                    .WithSet<Game>(games => games
                        .Any(g =>
                            g.Name == TestValidGameEditModel.Name &&
                      g.Description == TestValidGameEditModel.Description)
                    )).TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<GamesController>(c =>
                        c.Details(With.Any<int>())));

        [Fact]
        public void PostEditShouldReturnViewWithInvalidModelState()
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithData(TestGame)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Edit(TestInvalidGameEditModel, TestGame.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameServiceEditModel>());

        [Fact]
        public void PostEditShouldReturnBadRequestUponUnsuccessfulEdition()
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithData(TestArea)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Edit(TestValidGameEditModel, TestGame.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .AndAlso()
                .ShouldReturn()
                .BadRequest();

        [Fact]
        public void PostEditShouldReturnViewWithInvalidAreaId()
        => MyController<GamesController>
            .Instance(controller => controller
                .WithData(TestGame)
                .WithUser(TestUser.Identifier))
            .Calling(c => c.Edit(TestValidGameEditModelWithInvalidAreaId, TestGame.Id))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post))
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<GameServiceEditModel>());

        [Fact]
        public void DeleteShouldReturnErrorViewWithInvalidGameId()
            => MyController<GamesController>
                .Instance()
                .Calling(c => c.Delete(TestGame.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>());

        [Fact]
        public void DeleteShouldReturnUnauthorizedForUnauthorizedUsers()
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithData(TestGame)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Delete(TestGame.Id))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void DeleteShouldReturnRedirectWithValidId()
            => MyController<GamesController>
                .Instance(controller => controller
                    .WithData(TestGame)
                    .WithUser(user => user
                        .InRole(AdministratorRoleName)))
                .Calling(c => c.Delete(TestGame.Id))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Game>(games => games
                        .All(g =>
                            g.Name != TestValidGameFormModel.Name &&
                            g.Description != TestValidGameFormModel.Name)
                    )).TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<GamesController>(c =>
                        c.All(With.Any<int>())));

        [Fact]
        public void SearchShouldReturnCorrectViewWithModel()
            => MyController<GamesController>
                .Instance(controller => controller
                        .WithData(FiveGames))
                .Calling(c => c.Search(Guid.NewGuid()
                    .ToString(), DefaultPageIndex))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameFullModel>());

        [Fact]
        public void FilterShouldReturnCorrectViewWithModel()
            => MyController<GamesController>
                .Instance(controller => controller
                        .WithData(FiveGames))
                .Calling(c => c.Filter(new Random().Next(),
                    DefaultPageIndex))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameFullModel>());

        [Fact]
        public void MineShouldReturnCorrectViewWithModel()
            => MyController<GamesController>
                .Instance(controller => controller
                        .WithData(FiveGames))
                .Calling(c => c.Mine(DefaultPageIndex))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<GameFullModel>());
    }
}
