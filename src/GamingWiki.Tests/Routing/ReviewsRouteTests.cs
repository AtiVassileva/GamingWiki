using GamingWiki.Services.Models.Reviews;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models.Reviews;
using static GamingWiki.Tests.Data.Reviews;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace GamingWiki.Tests.Routing
{
    public class ReviewsRouteTests
    {
        [Fact]
        public void AllShouldBeMappedWithNoPageParameter()
            => MyRouting
                .Configuration()
                .ShouldMap("/Reviews/All")
                .To<ReviewsController>(c =>
                    c.All(With.No<int>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewFullModel>());

        [Fact]
        public void AllShouldBeMappedWithPageParameter()
            => MyRouting
                .Configuration()
                .ShouldMap("/Reviews/All?pageIndex=1")
                .To<ReviewsController>(c =>
                    c.All(1))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewFullModel>());

        [Fact]
        public void GetCreateShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Reviews/Create?gameId=1")
                .To<ReviewsController>(c => c.Create(1))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewFormModel>());

        [Fact]
        public void PostCreateShouldBeMapped()
        => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithMethod(HttpMethod.Post)
                .WithLocation("/Reviews/Create?gameId=1"))
            .To<ReviewsController>(c =>
                c.Create(With.Any<ReviewFormModel>(), 1));
        
        [Fact]
        public void GetEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap($"/Reviews/Edit?reviewId={TestReview.Id}")
                .To<ReviewsController>(c =>
                    c.Edit(TestReview.Id))
                .Which(controller => controller
                    .WithData(TestReview)
                    .WithUser(user => user.InRole(AdministratorRoleName)))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewDetailsServiceModel>());

        [Fact]
        public void PostEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"/Reviews/Edit?reviewId={TestReview.Id}"))
                .To<ReviewsController>(c =>
                    c.Edit(With.Any<ReviewFormModel>(),
                        TestReview.Id));
        
        [Fact]
        public void DeleteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Reviews/Delete?reviewId=1")
                .To<ReviewsController>(c =>
                    c.Delete(1));

        [Fact]
        public void GetSearchShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Reviews/Search?parameter=a")
                .To<ReviewsController>(c =>
                    c.Search("a", With.No<int>(), null))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewFullModel>());

        [Fact]
        public void GetSearchShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap("/Reviews/Search?parameter=a&pageIndex=1")
            .To<ReviewsController>(c =>
                c.Search("a", 1, null))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ReviewFullModel>());

        [Fact]
        public void GetSearchShouldBeMappedWithoutName()
            => MyRouting
                .Configuration()
                .ShouldMap("/Reviews/Search?parameter=a&pageIndex=1")
                .To<ReviewsController>(c =>
                    c.Search("a", 1, With.No<string>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewFullModel>());

        [Fact]
        public void GetSearchShouldBeMappedWithName()
            => MyRouting
                .Configuration()
                .ShouldMap("/Reviews/Search?parameter=a&pageIndex=1&name=test")
                .To<ReviewsController>(c =>
                    c.Search("a", 1, "test"))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewFullModel>());

        [Fact]
        public void PostSearchShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/Reviews/Search?searchCriteria=abc"))
                .To<ReviewsController>(c =>
                    c.Search("abc", With.No<int>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewFullModel>());

        [Fact]
        public void PostSearchShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithMethod(HttpMethod.Post)
                .WithLocation("/Reviews/Search?searchCriteria=abc&pageIndex=1"))
            .To<ReviewsController>(c =>
                c.Search("abc", 1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ReviewFullModel>());

        [Fact]
        public void MineShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Reviews/Mine")
                .To<ReviewsController>(c =>
                    c.Mine(With.No<int>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewFullModel>());

        [Fact]
        public void MineShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap("/Reviews/Mine?pageIndex=1")
            .To<ReviewsController>(c =>
                c.Mine(1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ReviewFullModel>());
    }
}
