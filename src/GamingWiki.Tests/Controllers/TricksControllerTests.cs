using System;
using System.Linq;
using GamingWiki.Models;
using GamingWiki.Services.Models.Tricks;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models.Tricks;
using MyTested.AspNetCore.Mvc;
using Xunit;
using static GamingWiki.Tests.Data.Tricks;
using static GamingWiki.Tests.Data.Games;
using static GamingWiki.Tests.Common.TestConstants;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Areas.Admin.AdminConstants;

namespace GamingWiki.Tests.Controllers
{
    public class TricksControllerTests
    {
        [Fact]
        public void TricksControllerShouldBeForAuthorizedUsersOnly()
       => MyController<TricksController>
           .ShouldHave()
           .Attributes(attributes => attributes
               .RestrictingForAuthorizedRequests());

        [Fact]
        public void AllShouldReturnCorrectViewWithModel()
            => MyController<TricksController>
                .Instance(controller => controller
                        .WithData(FiveTricks))
                .Calling(c => c.All(DefaultPageIndex))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickFullModel>());

        [Fact]
        public void GetCreateShouldReturnCorrectView()
            => MyController<TricksController>
                .Instance()
                .Calling(c => c.Create())
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickFormModel>());

        [Fact]
        public void PostCreateShouldBeForAuthorizedUsersAndReturnRedirectWithValidModel()
            => MyController<TricksController>
                .Instance(controller => controller
                    .WithData(TestGame)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Create(TestTrickValidFormModel))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                    .WithSet<Trick>(tricks =>
                        tricks.Any(t => t.GameId == TestGame.Id)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<TricksController>
                        (c => c.All(With.Any<int>())));

        [Fact]
        public void PostCreateShouldReturnViewWithInvalidModelState()
            => MyController<TricksController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Create(TestTrickValidFormModel))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickFormModel>());

        [Fact]
        public void GetEditShouldReturnErrorViewWithInvalidId()
            => MyController<TricksController>
                .Instance()
                .Calling(c => c.Edit(TestTrick.Id))
                .ShouldReturn()
                .View("Error");

        [Fact]
        public void GetEditShouldReturnCorrectViewWithValidId()
            => MyController<TricksController>
                .Instance(controller => controller
                    .WithData(TestTrick)
                    .WithUser(user => user
                        .InRole(AdministratorRoleName)))
                .Calling(c => c.Edit(TestTrick.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickServiceEditModel>());

        [Fact]
        public void GetEditShouldReturnUnauthorizedForUnauthorizedUsers()
            => MyController<TricksController>
                .Instance(controller => controller
                    .WithData(TestTrick)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Edit(TestTrick.Id))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void PostEditShouldReturnRedirectWithValidModel()
            => MyController<TricksController>
                .Instance(controller => controller
                    .WithData(TestTrick)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Edit(TestTrickValidEditModel, TestTrick.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                    .WithSet<Trick>(tricks => tricks
                        .Any(t => t.GameId == TestGame.Id)
                    )).TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<TricksController>(c =>
                        c.All(With.Any<int>())));

        [Fact]
        public void PostEditShouldReturnViewWithInvalidModelState()
            => MyController<TricksController>
                .Instance(controller => controller
                    .WithData(TestTrick)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Edit(TestTrickInvalidEditModel, TestTrick.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickServiceEditModel>());

        [Fact]
        public void PostEditShouldReturnErrorViewWithInvalidId()
            => MyController<TricksController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Edit(TestTrickValidEditModel, TestTrick.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .AndAlso()
                .ShouldReturn()
                .View("Error");

        [Fact]
        public void DeleteShouldReturnErrorViewWithInvalidTrickId()
            => MyController<TricksController>
                .Instance()
                .Calling(c => c.Delete(TestTrick.Id))
                .ShouldReturn()
                .View("Error");

        [Fact]
        public void DeleteShouldReturnUnauthorizedForUnauthorizedUsers()
            => MyController<TricksController>
                .Instance(controller => controller
                    .WithData(TestTrick)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Delete(TestTrick.Id))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void DeleteShouldReturnRedirectWithValidId()
            => MyController<TricksController>
                .Instance(controller => controller
                    .WithData(TestTrick)
                    .WithUser(user => user
                        .InRole(AdministratorRoleName)))
                .Calling(c => c.Delete(TestTrick.Id))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Trick>(tricks => tricks
                        .All(t => t.GameId != TestGame.Id)
                    )).TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<TricksController>(c =>
                        c.All(With.Any<int>())));

        [Fact]
        public void GetSearchShouldReturnCorrectViewWithModel()
            => MyController<TricksController>
                .Instance(controller => controller
                        .WithData(FiveTricks))
                .Calling(c => c.Search(Guid.NewGuid().ToString(),
                    DefaultPageIndex, null))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickFullModel>());

        [Fact]
        public void PostSearchShouldReturnCorrectViewWithModel()
            => MyController<TricksController>
                .Instance(controller => controller
                        .WithData(FiveTricks))
                .Calling(c => c.Search(Guid.NewGuid().ToString(),
                    DefaultPageIndex))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickFullModel>());

        [Fact]
        public void MineShouldReturnCorrectViewWithModel()
            => MyController<TricksController>
                .Instance(controller => controller
                        .WithData(FiveTricks))
                .Calling(c => c.Mine(DefaultPageIndex))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickFullModel>());
    }
}
