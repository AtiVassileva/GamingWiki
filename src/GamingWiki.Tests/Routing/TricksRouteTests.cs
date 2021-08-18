using GamingWiki.Services.Models.Tricks;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models.Tricks;
using static GamingWiki.Tests.Data.Tricks;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace GamingWiki.Tests.Routing
{
    public class TricksRouteTests
    {
        [Fact]
        public void AllShouldBeMappedWithNoPageParameter()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tricks/All")
                .To<TricksController>(c =>
                    c.All(With.No<int>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickFullModel>());

        [Fact]
        public void AllShouldBeMappedWithPageParameter()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tricks/All?pageIndex=1")
                .To<TricksController>(c =>
                    c.All(1))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickFullModel>());

        [Fact]
        public void GetCreateShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tricks/Create")
                .To<TricksController>(c => c.Create())
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickFormModel>());

        [Fact]
        public void PostCreateShouldBeMapped()
        => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithMethod(HttpMethod.Post)
                .WithLocation("/Tricks/Create"))
            .To<TricksController>(c =>
                c.Create(With.Any<TrickFormModel>()));
        
        [Fact]
        public void GetEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap($"/Tricks/Edit?trickId={TestTrick.Id}")
                .To<TricksController>(c =>
                    c.Edit(TestTrick.Id))
                .Which(controller => controller
                    .WithData(TestTrick)
                    .WithUser(user => user.InRole(AdministratorRoleName)))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickServiceEditModel>());

        [Fact]
        public void PostEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"/Tricks/Edit?trickId={TestTrick.Id}"))
                .To<TricksController>(c =>
                    c.Edit(With.Any<TrickServiceEditModel>(),
                        TestTrick.Id));
        
        [Fact]
        public void DeleteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tricks/Delete?trickId=1")
                .To<TricksController>(c =>
                    c.Delete(1));

        [Fact]
        public void GetSearchShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tricks/Search?parameter=a")
                .To<TricksController>(c =>
                    c.Search("a", With.No<int>(), null))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickFullModel>());

        [Fact]
        public void GetSearchShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap("/Tricks/Search?parameter=a&pageIndex=1")
            .To<TricksController>(c =>
                c.Search("a", 1, null))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<TrickFullModel>());

        [Fact]
        public void GetSearchShouldBeMappedWithoutName()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tricks/Search?parameter=a&pageIndex=1")
                .To<TricksController>(c =>
                    c.Search("a", 1, With.No<string>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickFullModel>());

        [Fact]
        public void GetSearchShouldBeMappedWithName()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tricks/Search?parameter=a&pageIndex=1&name=test")
                .To<TricksController>(c =>
                    c.Search("a", 1, "test"))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickFullModel>());

        [Fact]
        public void PostSearchShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/Tricks/Search?searchCriteria=abc"))
                .To<TricksController>(c =>
                    c.Search("abc", With.No<int>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickFullModel>());

        [Fact]
        public void PostSearchShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithMethod(HttpMethod.Post)
                .WithLocation("/Tricks/Search?searchCriteria=abc&pageIndex=1"))
            .To<TricksController>(c =>
                c.Search("abc", 1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<TrickFullModel>());

        [Fact]
        public void MineShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tricks/Mine")
                .To<TricksController>(c =>
                    c.Mine(With.No<int>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<TrickFullModel>());

        [Fact]
        public void MineShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap("/Tricks/Mine?pageIndex=1")
            .To<TricksController>(c =>
                c.Mine(1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<TrickFullModel>());
    }
}
