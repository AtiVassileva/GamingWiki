using System;
using System.Linq;
using GamingWiki.Models;
using GamingWiki.Services.Models.Discussions;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Discussions;
using MyTested.AspNetCore.Mvc;
using Xunit;
using static GamingWiki.Tests.Common.TestConstants;
using static GamingWiki.Tests.Data.Discussions;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Areas.Admin.AdminConstants;

namespace GamingWiki.Tests.Controllers
{
    public class DiscussionsControllerTests
    {
        [Fact]
        public void DiscussionsControllerShouldBeForAuthorizedUsersOnly()
        => MyController<DiscussionsController>
            .ShouldHave()
            .Attributes(attributes => attributes
                .RestrictingForAuthorizedRequests());

        [Fact]
        public void AllShouldReturnCorrectViewWithModel()
            => MyController<DiscussionsController>
                .Instance(controller => controller
                        .WithData(FiveDiscussions))
                .Calling(c => c.All(DefaultPageIndex))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionFullModel>());

        [Fact]
        public void GetCreateShouldReturnCorrectView()
            => MyController<DiscussionsController>
                .Instance()
                .Calling(c => c.Create())
                .ShouldReturn()
                .View();

        [Fact]
        public void PostCreateReturnRedirectWithValidModel()
            => MyController<DiscussionsController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Create(TestValidDiscussionFormModel))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                    .WithSet<Discussion>(discussions =>
                        discussions.Any(d =>
                        d.Name == TestValidDiscussionFormModel.Name
                        && d.Description == TestValidDiscussionFormModel.Description)
                    ))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<DiscussionsController>
                        (c => c.Details(With.Any<int>())));

        [Fact]
        public void PostCreateShouldReturnViewWithInvalidModelState()
            => MyController<DiscussionsController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Create(TestInvalidDiscussionFormModel))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionFormModel>());

        [Fact]
        public void DetailsShouldReturnErrorViewWithInvalidDiscussionId()
            => MyController<DiscussionsController>
                .Instance()
                .Calling(c => c.Details(TestDiscussion.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>());

        [Fact]
        public void DetailsShouldReturnCorrectViewWithValidDiscussionId()
            => MyController<DiscussionsController>
                .Instance(controller => controller
                    .WithData(TestDiscussion))
                .Calling(c => c.Details(TestDiscussion.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionServiceDetailsModel>());

        [Fact]
        public void GetEditShouldReturnErrorViewWithInvalidId()
            => MyController<DiscussionsController>
                .Instance()
                .Calling(c => c.Edit(TestDiscussion.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>());

        [Fact]
        public void GetEditShouldReturnCorrectViewWithValidId()
            => MyController<DiscussionsController>
                .Instance(controller => controller
                    .WithUser(user => user
                        .InRole(AdministratorRoleName))
                    .WithData(TestDiscussion))
                .Calling(c => c.Edit(TestDiscussion.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionServiceEditModel>());

        [Fact]
        public void GetEditShouldReturnUnauthorizedForUnauthorizedUsers()
            => MyController<DiscussionsController>
                .Instance(controller => controller
                    .WithData(TestDiscussion))
                .Calling(c => c.Edit(TestDiscussion.Id))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void PostEditShouldReturnRedirectWithValidModel()
            => MyController<DiscussionsController>
                .Instance(controller => controller
                   .WithData(TestDiscussion)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c
         .Edit(TestValidDiscussionEditModel, TestDiscussion.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                    .WithSet<Discussion>(discussions => discussions
                        .Any(d =>
                            d.Name == TestDiscussion.Name &&
                            d.Description == TestDiscussion.Description)
                    )).TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<DiscussionsController>(c =>
                        c.Details(With.Any<int>())));

        [Fact]
        public void PostEditShouldReturnViewWithInvalidModelState()
            => MyController<DiscussionsController>
                .Instance(controller => controller
                    .WithData(TestDiscussion)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Edit(TestInvalidDiscussionEditModel, TestDiscussion.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionServiceEditModel>());

        [Fact]
        public void PostEditShouldReturnBadRequestUponUnsuccessfulEdition()
            => MyController<DiscussionsController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Edit(TestValidDiscussionEditModel, TestDiscussion.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .AndAlso()
                .ShouldReturn()
                .BadRequest();

        [Fact]
        public void DeleteShouldReturnErrorViewWithInvalidDiscussionId()
            => MyController<DiscussionsController>
                .Instance()
                .Calling(c => c.Delete(TestDiscussion.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>());

        [Fact]
        public void DeleteShouldReturnUnauthorizedForUnauthorizedUsers()
            => MyController<DiscussionsController>
                .Instance(controller => controller
                    .WithData(TestDiscussion))
                .Calling(c => c.Delete(TestDiscussion.Id))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void DeleteShouldReturnRedirectWithValidId()
            => MyController<DiscussionsController>
                .Instance(controller => controller
                    .WithData(TestDiscussion)
                    .WithUser(user => user
                        .InRole(AdministratorRoleName)))
                .Calling(c => c.Delete(TestDiscussion.Id))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Discussion>(discussions => discussions
                        .All(d =>
                            d.Name != TestValidDiscussionFormModel.Name
                            && d.Description != TestValidDiscussionFormModel.Name
                            && d.Id != TestDiscussion.Id)
                    )).TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<DiscussionsController>(c =>
                        c.All(With.Any<int>())));

        [Fact]
        public void GetSearchShouldReturnCorrectViewWithModel()
            => MyController<DiscussionsController>
                .Instance(controller => controller
                        .WithData(FiveDiscussions))
                .Calling(a => a.Search(Guid.NewGuid().ToString(),
                    DefaultPageIndex, null))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionFullModel>());

        [Fact]
        public void PostSearchShouldReturnCorrectViewWithModel()
            => MyController<DiscussionsController>
                .Instance(controller => controller
                        .WithData(FiveDiscussions))
                .Calling(a => a.Search(Guid.NewGuid().ToString(),
                    DefaultPageIndex))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionFullModel>());
        
        [Fact]
        public void JoinShouldReturnErrorViewWithInvalidDiscussionId()
            => MyController<DiscussionsController>
                .Instance()
                .Calling(c => c.Join(TestDiscussion.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>());

        [Fact]
        public void JoinShouldRedirectWhenDiscussionIsFull()
            => MyController<DiscussionsController>
                .Instance(controller => controller
                    .WithData(TestDiscussion)
                    .WithData(FullTestUserDiscussion))
                .Calling(c => c.Join(TestDiscussion.Id))
                .ShouldHave()
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<DiscussionsController>(c =>
                        c.Details(With.Any<int>())));

        [Fact]
        public void JoinShouldRedirectWhenUserIsAlreadyInDiscussion()
        => MyController<DiscussionsController>
            .Instance(controller => controller
                .WithData(TestDiscussion)
                .WithUser(TestUser.Identifier)
                .WithData(TestUserDiscussionWithTestUser))
            .Calling(c => c.Join(TestDiscussion.Id))
            .ShouldHave()
            .TempData(tempData => tempData
                .ContainingEntryWithKey(GlobalMessageKey))
            .AndAlso()
            .ShouldReturn()
            .Redirect(redirect => redirect
                .To<DiscussionsController>(c =>
                    c.Chat(With.Any<int>())));

        [Fact]
        public void JoinShouldWorkCorrectlyWhenUserDoesNotParticipateInDiscussion()
            => MyController<DiscussionsController>
                .Instance(controller => controller
                    .WithData(TestDiscussion)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Join(TestDiscussion.Id))
                .ShouldHave()
                .Data(data => data
                    .WithSet<UserDiscussion>(discussions =>
                        discussions
                            .Any(ud => ud.UserId == TestUser.Identifier
                                       && ud.DiscussionId == TestDiscussion.Id)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<DiscussionsController>(d =>
                        d.Chat(With.Any<int>())));

        [Fact]
        public void LeaveShouldReturnErrorViewWithInvalidDiscussionId()
            => MyController<DiscussionsController>
                .Instance()
                .Calling(c => c.Leave(TestDiscussion.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>());

        [Fact]
        public void LeaveShouldRedirectWhenUserDoesNotParticipateInDiscussion()
            => MyController<DiscussionsController>
                .Instance(controller => controller
                    .WithData(TestDiscussion)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Leave(TestDiscussion.Id))
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<DiscussionsController>(c => c
                        .Details(With.Any<int>())));

        [Fact]
        public void LeaveShouldWorkCorrectlyWhenUserParticipatesInDiscussion()
            => MyController<DiscussionsController>
                .Instance(controller => controller
                    .WithData(TestDiscussion)
                    .WithUser(TestUser.Identifier)
                    .WithData(TestUserDiscussionWithTestUser))
                .Calling(c => c.Leave(TestDiscussion.Id))
                .ShouldHave()
                .Data(data => data
                    .WithSet<UserDiscussion>(userDiscussions => userDiscussions
                        .All(ud => ud.DiscussionId != TestDiscussion.Id
                                   && ud.UserId != TestUser.Identifier)))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<DiscussionsController>(c => c
                        .All(With.Any<int>())));

        [Fact]
        public void ChatShouldReturnErrorViewWithInvalidDiscussionId()
            => MyController<DiscussionsController>
                .Instance()
                .Calling(c => c.Chat(TestDiscussion.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>());

        [Fact]
        public void ChatShouldRedirectWhenUserDoesNotParticipateInDiscussion()
        => MyController<DiscussionsController>
            .Instance(controller => controller
                .WithData(TestDiscussion)
                .WithUser(TestUser.Identifier))
            .Calling(c => c.Chat(TestDiscussion.Id))
            .ShouldReturn()
            .Redirect(redirect => redirect
                .To<DiscussionsController>(c => c
                    .Details(With.Any<int>())));

        [Fact]
        public void ChatShouldReturnCorrectViewWhenUserIsDiscussionMember()
            => MyController<DiscussionsController>
                .Instance(controller => controller
                    .WithData(TestDiscussion)
                    .WithUser(TestUser.Identifier)
                    .WithData(TestUserDiscussionWithTestUser))
                .Calling(c => c.Chat(TestDiscussion.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionChatServiceModel>());

        [Fact]
        public void MineShouldReturnCorrectViewWithModel()
    => MyController<DiscussionsController>
        .Instance(controller => controller
                .WithData(FiveDiscussions))
        .Calling(c => c.Mine(DefaultPageIndex))
        .ShouldReturn()
        .View(view => view
            .WithModelOfType<DiscussionFullModel>());
    }
}
