using System;
using System.Linq;
using GamingWiki.Models;
using GamingWiki.Services.Models.Reviews;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models.Reviews;
using static GamingWiki.Tests.Common.TestConstants;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using static GamingWiki.Tests.Data.Reviews;
using static GamingWiki.Tests.Data.Games;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace GamingWiki.Tests.Controllers
{
    public class ReviewsControllerTests
    {
        [Fact]
        public void ReviewsControllerShouldBeForAuthorizedUsersOnly()
       => MyController<ReviewsController>
           .ShouldHave()
           .Attributes(attributes => attributes
               .RestrictingForAuthorizedRequests());

        [Fact]
        public void AllShouldReturnCorrectViewWithModel()
            => MyController<ReviewsController>
                .Instance(controller =>
                    controller.WithData(FiveReviews))
                .Calling(c => c.All(DefaultPageIndex))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewFullModel>());

        [Fact]
        public void GetCreateShouldReturnCorrectView()
            => MyController<ReviewsController>
                .Instance()
                .Calling(c => c.Create(TestGame.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewFormModel>());

        [Fact]
        public void PostCreateShouldBeForAuthorizedUsersAndReturnRedirectWithValidModel()
            => MyController<ReviewsController>
                .Instance(controller => controller
                    .WithData(TestGame)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Create(TestReviewValidFormModel, TestGame.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                    .WithSet<Review>(reviews =>
                        reviews.Any(r => r.GameId == TestGame.Id)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<GamesController>
                        (c => c.Details(With.Any<int>())));

        [Fact]
        public void PostCreateShouldReturnViewWithInvalidModelState()
            => MyController<ReviewsController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Create(TestReviewValidFormModel, TestGame.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewFormModel>());

        [Fact]
        public void GetEditShouldReturnErrorViewWithInvalidId()
            => MyController<ReviewsController>
                .Instance()
                .Calling(a => a.Edit(TestReview.Id))
                .ShouldReturn()
                .View("Error");

        [Fact]
        public void GetEditShouldReturnCorrectViewWithValidId()
            => MyController<ReviewsController>
                .Instance(instance => instance
                    .WithData(TestReview)
                    .WithUser(user => user
                        .InRole(AdministratorRoleName)))
                .Calling(a => a.Edit(TestReview.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewDetailsServiceModel>());

        [Fact]
        public void GetEditShouldReturnUnauthorizedForUnauthorizedUsers()
            => MyController<ReviewsController>
                .Instance(instance => instance
                    .WithData(TestReview)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Edit(TestReview.Id))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void PostEditShouldReturnRedirectWithValidModel()
            => MyController<ReviewsController>
                .Instance(controller => controller
                    .WithData(TestReview)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Edit(TestReviewValidFormModel, TestReview.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                    .WithSet<Review>(reviews => reviews
                        .Any(r => r.GameId == TestGame.Id)
                    )).TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<GamesController>(c =>
                        c.Details(With.Any<int>())));

        [Fact]
        public void PostEditShouldReturnViewWithInvalidModelState()
            => MyController<ReviewsController>
                .Instance(controller => controller
                    .WithData(TestReview)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Edit(TestReviewInvalidFormModel, TestReview.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewDetailsServiceModel>());

        [Fact]
        public void PostEditShouldReturnErrorViewWithInvalidId()
            => MyController<ReviewsController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Edit(TestReviewValidFormModel, TestReview.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .AndAlso()
                .ShouldReturn()
                .View("Error");

        [Fact]
        public void DeleteShouldReturnErrorViewWithInvalidReviewId()
            => MyController<ReviewsController>
                .Instance()
                .Calling(c => c.Delete(TestReview.Id))
                .ShouldReturn()
                .View("Error");

        [Fact]
        public void DeleteShouldReturnUnauthorizedForUnauthorizedUsers()
            => MyController<ReviewsController>
                .Instance(instance => instance
                    .WithData(TestReview)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Delete(TestReview.Id))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void DeleteShouldReturnRedirectWithValidId()
            => MyController<ReviewsController>
                .Instance(controller => controller
                    .WithData(TestReview)
                    .WithUser(user => user
                        .InRole(AdministratorRoleName)))
                .Calling(c => c.Delete(TestReview.Id))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Review>(reviews => reviews
                        .All(r => r.GameId != TestGame.Id)
                    )).TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ReviewsController>(c =>
                        c.All(With.Any<int>())));

        [Fact]
        public void GetSearchShouldReturnCorrectViewWithModel()
            => MyController<ReviewsController>
                .Instance(instance =>
                    instance.WithData(FiveReviews))
                .Calling(a => a.Search(Guid.NewGuid().ToString(), 
                    DefaultPageIndex, null))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewFullModel>());

        [Fact]
        public void PostSearchShouldReturnCorrectViewWithModel()
            => MyController<ReviewsController>
                .Instance(instance =>
                    instance.WithData(FiveReviews))
                .Calling(c => c.Search(Guid.NewGuid().ToString(), 
                    DefaultPageIndex))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewFullModel>());

        [Fact]
        public void MineShouldReturnCorrectViewWithModel()
            => MyController<ReviewsController>
                .Instance(instance =>
                    instance.WithData(FiveReviews))
                .Calling(c => c.Mine(DefaultPageIndex))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewFullModel>());
    }
}
