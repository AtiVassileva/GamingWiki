using GamingWiki.Services.Models.Tricks;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models.Tricks;
using static GamingWiki.Tests.Data.Tricks;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using MyTested.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace GamingWiki.Tests.Pipeline
{
    public class TricksPipelineTests
    {
        [Fact]
        public void AllWithoutPageShouldReturnCorrectViewWithCorrectModel()
       => MyPipeline
           .Configuration()
           .ShouldMap(request => request
               .WithLocation("/Tricks/All")
               .WithUser()
               .WithAntiForgeryToken())
           .To<TricksController>(c => c.All(With.No<int>()))
           .Which(controller => controller
               .WithData(FiveTricks))
           .ShouldReturn()
           .View(view => view
               .WithModelOfType<TrickFullModel>()
               .Passing(trickListing =>
               {
                   trickListing.Tricks.Count.ShouldBe(0);
                   trickListing.Tricks.PageIndex.ShouldBe(0);
                   trickListing.Tricks.TotalPages.ShouldBe(2);
               }));

        [Theory]
        [InlineData(1, 0, 2)]
        public void AllWithPageShouldReturnCorrectViewWithCorrectModel(int pageIndex, int expectedCountOnPage, int totalPages)
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Tricks/All?pageIndex={pageIndex}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<TricksController>(c => c.All(pageIndex))
                .Which(controller => controller
                    .WithData(FiveTricks))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickFullModel>()
                    .Passing(trickListing =>
                    {
                        trickListing.Tricks.Count.ShouldBe(expectedCountOnPage);
                        trickListing.Tricks.PageIndex.ShouldBe(pageIndex);
                        trickListing.Tricks.TotalPages.ShouldBe(totalPages);
                    }));

        [Fact]
        public void GetCreateShouldBeMappedAndReturnCorrectView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Tricks/Create")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<TricksController>(c => c.Create())
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickFormModel>());

        [Fact]
        public void GetEditShouldBeMappedAndReturnCorrectViewWithValidId()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Tricks/Edit?trickId={TestTrick.Id}")
                    .WithUser(user => user.InRole(AdministratorRoleName))
                    .WithAntiForgeryToken())
                .To<TricksController>(c => c.Edit(TestTrick.Id))
                .Which(controller => controller
                    .WithData(TestTrick))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickServiceEditModel>());

        [Fact]
        public void GetEditShouldBeMappedAndReturnUnauthorizedForUnauthorizedUsers()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Tricks/Edit?trickId={TestTrick.Id}")
                    .WithUser(TestUser.Identifier)
                    .WithAntiForgeryToken())
                .To<TricksController>(c => c.Edit(TestTrick.Id))
                .Which(controller => controller
                    .WithData(TestTrick))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void GetEditShouldBeMappedAndReturnErrorViewWithInvalidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Tricks/Edit?trickId={TestTrick.Id}")
                .WithUser(TestUser.Identifier)
                .WithAntiForgeryToken())
            .To<TricksController>(c => c.Edit(TestTrick.Id))
            .Which()
            .ShouldReturn()
            .View("Error");

        [Fact]
        public void DeleteShouldBeMappedAndRedirectUponSuccessfulAction()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Tricks/Delete?trickId={TestTrick.Id}")
                    .WithUser(user => user.InRole(AdministratorRoleName))
                    .WithAntiForgeryToken())
                .To<TricksController>(c => c.Delete(TestTrick.Id))
                .Which(controller => controller
                    .WithData(TestTrick))
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<TricksController>(c => c
                        .All(With.Any<int>())));

        [Fact]
        public void DeleteShouldBeMappedAndReturnUnauthorizedForUnauthorizedUsers()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Tricks/Delete?trickId={TestTrick.Id}")
                .WithUser(TestUser.Identifier)
                .WithAntiForgeryToken())
            .To<TricksController>(c => c.Delete(TestTrick.Id))
            .Which(controller => controller
                .WithData(TestTrick))
            .ShouldReturn()
            .Unauthorized();

        [Fact]
        public void DeleteShouldBeMappedAndReturnErrorViewWithInvalidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Tricks/Delete?trickId={TestTrick.Id}")
                .WithUser(user => user.InRole(AdministratorRoleName))
                .WithAntiForgeryToken())
            .To<TricksController>(c =>
                c.Delete(TestTrick.Id))
            .Which()
            .ShouldReturn()
            .View("Error");

        [Fact]
        public void GetSearchShouldBeMappedAndReturnCorrectViewWithPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Tricks/Search?parameter=a&pageIndex=1")
                .WithUser()
                .WithAntiForgeryToken())
            .To<TricksController>(c =>
                c.Search("a", 1, With.No<string>()))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<TrickFullModel>());

        [Fact]
        public void GetSearchShouldBeMappedAndReturnCorrectViewWithoutPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
            .WithLocation("/Tricks/Search?parameter=a")
            .WithUser()
            .WithAntiForgeryToken())
            .To<TricksController>(c =>
        c.Search("a", With.No<int>(), With.No<string>()))
            .Which()
            .ShouldReturn()
            .View(view => view
            .WithModelOfType<TrickFullModel>());

        [Fact]
        public void GetSearchShouldBeMappedAndReturnCorrectViewWithName()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation("/Tricks/Search?parameter=a&pageIndex=1&name=test")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<TricksController>(c =>
                    c.Search("a", 1, "test"))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickFullModel>());

        [Fact]
        public void PostSearchShouldBeMappedAndReturnCorrectViewWithoutPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Tricks/Search?searchCriteria=abc")
                .WithMethod(HttpMethod.Post)
                .WithUser()
                .WithAntiForgeryToken())
            .To<TricksController>(c =>
                c.Search("abc", With.No<int>()))
            .Which()
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post))
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<TrickFullModel>());

        [Fact]
        public void PostSearchShouldBeMappedAndReturnCorrectViewWithPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Tricks/Search?searchCriteria=abc&pageIndex=1")
                .WithMethod(HttpMethod.Post)
                .WithUser()
                .WithAntiForgeryToken())
            .To<TricksController>(c =>
                c.Search("abc", 1))
            .Which()
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post))
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<TrickFullModel>());

        [Fact]
        public void MineShouldBeMappedAndReturnCorrectViewWithPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Tricks/Mine?pageIndex=1")
                .WithUser()
                .WithAntiForgeryToken())
            .To<TricksController>(c =>
                c.Mine(1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<TrickFullModel>());

        [Fact]
        public void MineShouldBeMappedAndReturnCorrectViewWithoutPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Tricks/Mine")
                .WithUser()
                .WithAntiForgeryToken())
            .To<TricksController>(c =>
                c.Mine(With.No<int>()))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<TrickFullModel>());
    }
}
