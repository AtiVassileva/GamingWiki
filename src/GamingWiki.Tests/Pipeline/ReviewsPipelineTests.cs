using System.Collections.Generic;
using GamingWiki.Services.Models.Reviews;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Reviews;
using static GamingWiki.Tests.Data.Reviews;
using static GamingWiki.Tests.Data.Games;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Common.ExceptionMessages;
using MyTested.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace GamingWiki.Tests.Pipeline
{
    public class ReviewsPipelineTests
    {
        [Fact]
        public void AllWithoutPageShouldReturnCorrectViewWithCorrectModel()
       => MyPipeline
           .Configuration()
           .ShouldMap(request => request
               .WithLocation("/Reviews/All")
               .WithUser()
               .WithAntiForgeryToken())
           .To<ReviewsController>(c => c.All(With.No<int>()))
           .Which(controller => controller
               .WithData(FiveReviews))
           .ShouldReturn()
           .View(view => view
               .WithModelOfType<ReviewFullModel>(m =>
               {
                   m.Reviews.ShouldBeOfType(typeof(PaginatedList<ReviewDetailsServiceModel>));
                   m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
               })
               .Passing(reviewListing =>
               {
                   reviewListing.Reviews.Count.ShouldBe(0);
                   reviewListing.Reviews.PageIndex.ShouldBe(0);
                   reviewListing.Reviews.TotalPages.ShouldBe(2);
               }));

        [Theory]
        [InlineData(1, 0, 2)]
        public void AllWithPageShouldReturnCorrectViewWithCorrectModel(int pageIndex, int expectedCountOnPage, int totalPages)
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Reviews/All?pageIndex={pageIndex}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<ReviewsController>(c => c.All(pageIndex))
                .Which(controller => controller
                    .WithData(FiveReviews))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewFullModel>(m =>
                    {
                        m.Reviews.ShouldBeOfType(typeof(PaginatedList<ReviewDetailsServiceModel>));
                        m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                    })
                    .Passing(reviewListing =>
                    {
                        reviewListing.Reviews.Count.ShouldBe(expectedCountOnPage);
                        reviewListing.Reviews.PageIndex.ShouldBe(pageIndex);
                        reviewListing.Reviews.TotalPages.ShouldBe(totalPages);
                    }));

        [Fact]
        public void GetCreateShouldBeMappedAndReturnCorrectView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Reviews/Create?gameId={TestGame.Id}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<ReviewsController>(c => c.Create(TestGame.Id))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewFormModel>());

        [Fact]
        public void PostCreateShouldBeMappedAndHaveInvalidModelStateAndReturnView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"/Reviews/Create?gameId={TestGame.Id}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<ReviewsController>(c => c.Create(TestGame.Id))
                .Which()
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewFormModel>());

        [Fact]
        public void GetEditShouldBeMappedAndReturnCorrectViewWithValidId()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Reviews/Edit?reviewId={TestReview.Id}")
                    .WithUser(user => user.InRole(AdministratorRoleName))
                    .WithAntiForgeryToken())
                .To<ReviewsController>(c => c.Edit(TestReview.Id))
                .Which(controller => controller
                    .WithData(TestReview))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewDetailsServiceModel>());

        [Fact]
        public void GetEditShouldBeMappedAndReturnUnauthorizedForUnauthorizedUsers()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Reviews/Edit?reviewId={TestReview.Id}")
                    .WithUser(TestUser.Identifier)
                    .WithAntiForgeryToken())
                .To<ReviewsController>(c => c.Edit(TestReview.Id))
                .Which(controller => controller
                    .WithData(TestReview))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void GetEditShouldBeMappedAndReturnErrorViewWithInvalidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Reviews/Edit?reviewId={TestReview.Id}")
                .WithUser(TestUser.Identifier)
                .WithAntiForgeryToken())
            .To<ReviewsController>(c => c.Edit(TestReview.Id))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ErrorViewModel>(m => m
                    .Message == NonExistingReviewExceptionMessage));

        [Fact]
        public void PostEditShouldBeMappedAndHaveInvalidModelStateAndReturnView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"/Reviews/Edit?reviewId={TestReview.Id}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<ReviewsController>(c => c.Edit(new ReviewFormModel(), TestReview.Id))
                .Which(controller => controller
                    .WithData(TestReview))
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewDetailsServiceModel>());

        [Fact]
        public void DeleteShouldBeMappedAndRedirectUponSuccessfulAction()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Reviews/Delete?reviewId={TestReview.Id}")
                    .WithUser(user => user.InRole(AdministratorRoleName))
                    .WithAntiForgeryToken())
                .To<ReviewsController>(c => c.Delete(TestReview.Id))
                .Which(controller => controller
                    .WithData(TestReview))
                .ShouldHave()
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ReviewsController>(c => c
                        .All(With.Any<int>())));

        [Fact]
        public void DeleteShouldBeMappedAndReturnUnauthorizedForUnauthorizedUsers()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Reviews/Delete?reviewId={TestReview.Id}")
                .WithUser(TestUser.Identifier)
                .WithAntiForgeryToken())
            .To<ReviewsController>(c => c.Delete(TestReview.Id))
            .Which(controller => controller
                .WithData(TestReview))
            .ShouldReturn()
            .Unauthorized();

        [Fact]
        public void DeleteShouldBeMappedAndReturnErrorViewWithInvalidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Reviews/Delete?reviewId={TestReview.Id}")
                .WithUser(user => user.InRole(AdministratorRoleName))
                .WithAntiForgeryToken())
            .To<ReviewsController>(c =>
                c.Delete(TestReview.Id))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ErrorViewModel>(m => m
                    .Message == NonExistingReviewExceptionMessage));

        [Fact]
        public void GetSearchShouldBeMappedAndReturnCorrectViewWithPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Reviews/Search?parameter=a&pageIndex=1")
                .WithUser()
                .WithAntiForgeryToken())
            .To<ReviewsController>(c =>
                c.Search("a", 1, With.No<string>()))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ReviewFullModel>(m =>
                {
                    m.Reviews.ShouldBeOfType(typeof(PaginatedList<ReviewDetailsServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));

        [Fact]
        public void GetSearchShouldBeMappedAndReturnCorrectViewWithoutPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
            .WithLocation("/Reviews/Search?parameter=a")
            .WithUser()
            .WithAntiForgeryToken())
            .To<ReviewsController>(c =>
        c.Search("a", With.No<int>(), With.No<string>()))
            .Which()
            .ShouldReturn()
            .View(view => view
            .WithModelOfType<ReviewFullModel>(m =>
            {
                m.Reviews.ShouldBeOfType(typeof(PaginatedList<ReviewDetailsServiceModel>));
                m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
            }));

        [Fact]
        public void GetSearchShouldBeMappedAndReturnCorrectViewWithName()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation("/Reviews/Search?parameter=a&pageIndex=1&name=test")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<ReviewsController>(c =>
                    c.Search("a", 1, "test"))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ReviewFullModel>(m =>
                    {
                        m.Reviews.ShouldBeOfType(typeof(PaginatedList<ReviewDetailsServiceModel>));
                        m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                    }));

        [Fact]
        public void PostSearchShouldBeMappedAndReturnCorrectViewWithoutPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Reviews/Search?searchCriteria=abc")
                .WithMethod(HttpMethod.Post)
                .WithUser()
                .WithAntiForgeryToken())
            .To<ReviewsController>(c =>
                c.Search("abc", With.No<int>()))
            .Which()
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post))
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ReviewFullModel>(m =>
                {
                    m.Reviews.ShouldBeOfType(typeof(PaginatedList<ReviewDetailsServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));

        [Fact]
        public void PostSearchShouldBeMappedAndReturnCorrectViewWithPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Reviews/Search?searchCriteria=abc&pageIndex=1")
                .WithMethod(HttpMethod.Post)
                .WithUser()
                .WithAntiForgeryToken())
            .To<ReviewsController>(c =>
                c.Search("abc", 1))
            .Which()
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post))
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ReviewFullModel>(m =>
                {
                    m.Reviews.ShouldBeOfType(typeof(PaginatedList<ReviewDetailsServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));

        [Fact]
        public void MineShouldBeMappedAndReturnCorrectViewWithPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Reviews/Mine?pageIndex=1")
                .WithUser()
                .WithAntiForgeryToken())
            .To<ReviewsController>(c =>
                c.Mine(1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ReviewFullModel>(m =>
                {
                    m.Reviews.ShouldBeOfType(typeof(PaginatedList<ReviewDetailsServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));

        [Fact]
        public void MineShouldBeMappedAndReturnCorrectViewWithoutPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Reviews/Mine")
                .WithUser()
                .WithAntiForgeryToken())
            .To<ReviewsController>(c =>
                c.Mine(With.No<int>()))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ReviewFullModel>(m =>
                {
                    m.Reviews.ShouldBeOfType(typeof(PaginatedList<ReviewDetailsServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));
    }
}
