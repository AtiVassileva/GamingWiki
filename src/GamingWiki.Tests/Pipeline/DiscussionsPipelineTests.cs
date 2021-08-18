using System.Collections.Generic;
using GamingWiki.Services.Models.Classes;
using GamingWiki.Services.Models.Discussions;
using GamingWiki.Tests.Data;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Discussions;
using static GamingWiki.Tests.Data.Discussions;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using MyTested.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace GamingWiki.Tests.Pipeline
{
    public class DiscussionsPipelineTests
    {
        [Fact]
        public void AllWithoutPageShouldReturnCorrectViewWithCorrectModel()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Discussions/All")
                .WithUser()
                .WithAntiForgeryToken())
            .To<DiscussionsController>(c => c.All(With.No<int>()))
            .Which(controller => controller
                .WithData(FiveDiscussions))
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<DiscussionFullModel>(m =>
                {
                    m.Discussions.ShouldBeOfType(typeof(PaginatedList<DiscussionAllServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                })
                .Passing(discussionListing =>
                {
                    discussionListing.Discussions.Count.ShouldBe(0);
                    discussionListing.Discussions.PageIndex.ShouldBe(0);
                    discussionListing.Discussions.TotalPages.ShouldBe(1);
                }));

        [Theory]
        [InlineData(1, 0, 1)]
        public void AllWithPageShouldReturnCorrectViewWithCorrectModel(int pageIndex, int expectedCountOnPage, int totalPages)
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Discussions/All?pageIndex={pageIndex}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<DiscussionsController>(c => c.All(pageIndex))
                .Which(controller => controller
                    .WithData(FiveDiscussions))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionFullModel>(m =>
                    {
                        m.Discussions.ShouldBeOfType(typeof(PaginatedList<DiscussionAllServiceModel>));
                        m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                    })
                    .Passing(discussionListing =>
                    {
                        discussionListing.Discussions.Count.ShouldBe(expectedCountOnPage);
                        discussionListing.Discussions.PageIndex.ShouldBe(pageIndex);
                        discussionListing.Discussions.TotalPages.ShouldBe(totalPages);
                    }));

        [Fact]
        public void GetCreateShouldBeMappedAndReturnCorrectView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation("/Discussions/Create")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<DiscussionsController>(c => c.Create())
                .Which()
                .ShouldReturn()
                .View();

        [Fact]
        public void PostCreateShouldBeMappedAndHaveInvalidModelStateAndReturnView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/Discussions/Create")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<DiscussionsController>(c => c.Create(new DiscussionFormModel()))
                .Which()
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionFormModel>());

        [Fact]
        public void DetailsShouldBeMappedAndReturnCorrectViewWithValidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Discussions/Details?discussionId={TestDiscussion.Id}")
                .WithUser()
                .WithAntiForgeryToken())
            .To<DiscussionsController>(c => c.Details(TestDiscussion.Id))
            .Which(controller => controller
                .WithData(TestDiscussion))
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<DiscussionServiceDetailsModel>());

        [Fact]
        public void DetailsShouldBeMappedAndReturnErrorViewWithInvalidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Discussions/Details?discussionId={TestDiscussion.Id}")
                .WithUser()
                .WithAntiForgeryToken())
            .To<DiscussionsController>(c => c.Details(TestDiscussion.Id))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ErrorViewModel>());

        [Fact]
        public void GetEditShouldBeMappedAndReturnCorrectViewWithValidId()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Discussions/Edit?discussionId={TestDiscussion.Id}")
                    .WithUser(user => user.InRole(AdministratorRoleName))
                    .WithAntiForgeryToken())
                .To<DiscussionsController>(c => c.Edit(TestDiscussion.Id))
                .Which(controller => controller
                    .WithData(TestDiscussion))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionServiceEditModel>());

        [Fact]
        public void GetEditShouldBeMappedAndReturnUnauthorizedForUnauthorizedUsers()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Discussions/Edit?discussionId={TestDiscussion.Id}")
                    .WithUser(TestUser.Identifier)
                    .WithAntiForgeryToken())
                .To<DiscussionsController>(c => c.Edit(TestDiscussion.Id))
                .Which(controller => controller
                    .WithData(TestDiscussion))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void GetEditShouldBeMappedAndReturnErrorViewWithInvalidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Discussions/Edit?discussionId={TestDiscussion.Id}")
                .WithUser(TestUser.Identifier)
                .WithAntiForgeryToken())
            .To<DiscussionsController>(c => c.Edit(TestDiscussion.Id))
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
                    .WithLocation($"/Discussions/Edit?discussionId={TestDiscussion.Id}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<DiscussionsController>(c => c.Edit(new DiscussionServiceEditModel(), TestDiscussion.Id))
                .Which(controller => controller
                    .WithData(TestDiscussion))
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionServiceEditModel>());

        [Fact]
        public void DeleteShouldBeMappedAndRedirectUponSuccessfulAction()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Discussions/Delete?discussionId={TestDiscussion.Id}")
                    .WithUser(user => user.InRole(AdministratorRoleName))
                    .WithAntiForgeryToken())
                .To<DiscussionsController>(c => c.Delete(TestDiscussion.Id))
                .Which(controller => controller
                    .WithData(TestDiscussion))
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<DiscussionsController>(c => c
                        .All(With.Any<int>())));

        [Fact]
        public void DeleteShouldBeMappedAndReturnUnauthorizedForUnauthorizedUsers()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Discussions/Delete?discussionId={TestDiscussion.Id}")
                .WithUser(TestUser.Identifier)
                .WithAntiForgeryToken())
            .To<DiscussionsController>(c => c.Delete(TestDiscussion.Id))
            .Which(controller => controller
                .WithData(TestDiscussion))
            .ShouldReturn()
            .Unauthorized();

        [Fact]
        public void DeleteShouldBeMappedAndReturnErrorViewWithInvalidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Discussions/Delete?discussionId={TestDiscussion.Id}")
                .WithUser(user => user.InRole(AdministratorRoleName))
                .WithAntiForgeryToken())
            .To<DiscussionsController>(c =>
                c.Delete(TestDiscussion.Id))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ErrorViewModel>());

        [Fact]
        public void GetSearchShouldBeMappedAndReturnCorrectViewWithPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Discussions/Search?parameter=a&pageIndex=1")
                .WithUser()
                .WithAntiForgeryToken())
            .To<DiscussionsController>(c =>
                c.Search("a", 1, With.No<string>()))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<DiscussionFullModel>(m =>
                {
                    m.Discussions.ShouldBeOfType(typeof(PaginatedList<DiscussionAllServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));

        [Fact]
        public void GetSearchShouldBeMappedAndReturnCorrectViewWithoutPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
            .WithLocation("/Discussions/Search?parameter=a")
            .WithUser()
            .WithAntiForgeryToken())
            .To<DiscussionsController>(c =>
        c.Search("a", With.No<int>(), With.No<string>()))
            .Which()
            .ShouldReturn()
            .View(view => view
            .WithModelOfType<DiscussionFullModel>(m =>
            {
                m.Discussions.ShouldBeOfType(typeof(PaginatedList<DiscussionAllServiceModel>));
                m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
            }));

        [Fact]
        public void GetSearchShouldBeMappedAndReturnCorrectViewWithName()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation("/Discussions/Search?parameter=a&pageIndex=1&name=test")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<DiscussionsController>(c =>
                    c.Search("a", 1, "test"))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionFullModel>(m =>
                    {
                        m.Discussions.ShouldBeOfType(typeof(PaginatedList<DiscussionAllServiceModel>));
                        m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                    }));

        [Fact]
        public void PostSearchShouldBeMappedAndReturnCorrectViewWithoutPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Discussions/Search?searchCriteria=abc")
                .WithMethod(HttpMethod.Post)
                .WithUser()
                .WithAntiForgeryToken())
            .To<DiscussionsController>(c =>
                c.Search("abc", With.No<int>()))
            .Which()
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post))
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<DiscussionFullModel>(m =>
                {
                    m.Discussions.ShouldBeOfType(typeof(PaginatedList<DiscussionAllServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));

        [Fact]
        public void PostSearchShouldBeMappedAndReturnCorrectViewWithPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Discussions/Search?searchCriteria=abc&pageIndex=1")
                .WithMethod(HttpMethod.Post)
                .WithUser()
                .WithAntiForgeryToken())
            .To<DiscussionsController>(c =>
                c.Search("abc", 1))
            .Which()
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post))
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<DiscussionFullModel>(m =>
                {
                    m.Discussions.ShouldBeOfType(typeof(PaginatedList<DiscussionAllServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));
        
        [Fact]
        public void JoinShouldBeMappedAndRedirectUponSuccessfulAction()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Discussions/Join?discussionId={TestDiscussion.Id}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<DiscussionsController>(c => c.Join(TestDiscussion.Id))
                .Which(controller => controller
                    .WithData(TestDiscussion))
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<DiscussionsController>(c => c.Chat(TestDiscussion.Id)));
        
        [Fact]
        public void JoinShouldBeMappedAndRedirectWhenUserParticipatesInDiscussion()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Discussions/Join?discussionId={TestDiscussion.Id}")
                    .WithUser(Users.TestUser.Id)
                    .WithAntiForgeryToken())
                .To<DiscussionsController>(c => c.Join(TestDiscussion.Id))
                .Which(controller => controller
                    .WithData(TestDiscussion))
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<DiscussionsController>(c => c.Chat(TestDiscussion.Id)));

        [Fact]
        public void JoinShouldBeMappedAndReturnErrorViewWithInvalidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Discussions/Join?discussionId={TestDiscussion.Id}")
                .WithUser()
                .WithAntiForgeryToken())
            .To<DiscussionsController>(c => c.Join(TestDiscussion.Id))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ErrorViewModel>());

        [Fact]
        public void JoinShouldBeaMappedAndShouldRedirectWhenDiscussionIsFull()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Discussions/Join?discussionId={TestDiscussion.Id}")
                .WithUser()
                .WithAntiForgeryToken())
            .To<DiscussionsController>(c => c.Join(TestDiscussion.Id))
            .Which(controller => controller
                .WithData(TestDiscussion)
                .WithData(FullTestUserDiscussion))
            .ShouldReturn()
            .Redirect(redirect => redirect
                .To<DiscussionsController>(c => c.Details(TestDiscussion.Id)));

        [Fact]
        public void LeaveShouldBeMappedAndRedirectUponSuccessfulAction()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Discussions/Leave?discussionId={TestDiscussion.Id}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<DiscussionsController>(c => c.Leave(TestDiscussion.Id))
                .Which(controller => controller
                    .WithData(TestDiscussion)
                    .WithData(TestUserDiscussionWithTestUser))
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<DiscussionsController>(c => c.All(With.Any<int>())));

        [Fact]
        public void LeaveShouldBeMappedAndRedirectWhenUserDoesNotParticipate()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Discussions/Leave?discussionId={TestDiscussion.Id}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<DiscussionsController>(c => c.Leave(TestDiscussion.Id))
                .Which(controller => controller
                    .WithData(TestDiscussion))
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<DiscussionsController>(c => c.Details(TestDiscussion.Id)));

        [Fact]
        public void LeaveShouldReturnErrorViewWithInvalidId()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Discussions/Leave?discussionId={TestDiscussion.Id}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<DiscussionsController>(c => c.Leave(TestDiscussion.Id))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>());
[Fact]
        public void ChatShouldReturnErrorViewWithInvalidId()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Discussions/Chat?discussionId={TestDiscussion.Id}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<DiscussionsController>(c => c.Chat(TestDiscussion.Id))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>());

        [Fact]
        public void ChatShouldRedirectWhenUserDoesNotParticipate()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Discussions/Chat?discussionId={TestDiscussion.Id}")
                .WithUser()
                .WithAntiForgeryToken())
            .To<DiscussionsController>(c => c.Chat(TestDiscussion.Id))
            .Which(controller => controller
                .WithData(TestDiscussion))
            .ShouldReturn()
            .Redirect(redirect => redirect
                .To<DiscussionsController>(c => c.Details(TestDiscussion.Id)));

        [Fact]
        public void ChatShouldReturnCorrectViewUponSuccessfulAction()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Discussions/Chat?discussionId={TestDiscussion.Id}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<DiscussionsController>(c => c.Chat(TestDiscussion.Id))
                .Which(controller => controller
                    .WithData(TestDiscussion)
                    .WithData(TestUserDiscussionWithTestUser))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionChatServiceModel>());

        [Fact]
        public void MineShouldBeMappedAndReturnCorrectViewWithPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Discussions/Mine?pageIndex=1")
                .WithUser()
                .WithAntiForgeryToken())
            .To<DiscussionsController>(c =>
                c.Mine(1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<DiscussionFullModel>(m =>
                {
                    m.Discussions.ShouldBeOfType(typeof(PaginatedList<DiscussionAllServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));

        [Fact]
        public void MineShouldBeMappedAndReturnCorrectViewWithoutPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Discussions/Mine")
                .WithUser()
                .WithAntiForgeryToken())
            .To<DiscussionsController>(c =>
                c.Mine(With.No<int>()))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<DiscussionFullModel>(m =>
                {
                    m.Discussions.ShouldBeOfType(typeof(PaginatedList<DiscussionAllServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));
    }
}
