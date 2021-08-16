using System;
using GamingWiki.Models;
using GamingWiki.Web.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;
using System.Linq;
using static GamingWiki.Tests.Data.Replies;
using static GamingWiki.Tests.Data.Comments;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Areas.Admin.AdminConstants;

namespace GamingWiki.Tests.Controllers
{
    public class RepliesControllerTests
    {
        [Fact]
        public void RepliesControllerShouldBeForAuthorizedUsersOnly()
            => MyController<RepliesController>
                .ShouldHave()
                .Attributes(attributes => attributes
                    .RestrictingForAuthorizedRequests());

        [Fact]
        public void AddShouldReturnRedirectWithValidModel()
            => MyController<RepliesController>
                .Instance(controller => controller
                    .WithData(TestComment))
                .Calling(c => c.Add(TestValidReplyFormModel, TestComment.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                    .WithSet<Reply>(replies =>
                        replies.Any(r =>
                            r.Content == TestValidCommentFormModel.Content
                            && r.CommentId == TestComment.Id)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ArticlesController>
                        (c => c.Details(With.Any<int>())));

        [Fact]
        public void AddShouldRedirectWithInvalidModelState()
            => MyController<RepliesController>
                .Instance(controller => controller
                    .WithData(TestComment)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Add(TestInvalidReplyFormModel, TestComment.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ArticlesController>
                        (c => c.Details(With.Any<int>())));

        [Fact]
        public void AddShouldReturnErrorViewWithInvalidCommentId()
            => MyController<RepliesController>
                .Instance()
                .Calling(c => c.Add(TestValidReplyFormModel,
                    TestComment.Id))
                .ShouldReturn()
                .View("Error");

        [Fact]
        public void DeleteShouldReturnErrorViewWithInvalidReplyId()
            => MyController<RepliesController>
                .Instance()
                .Calling(c => c.Delete(TestReply.Id))
                .ShouldReturn()
                .View("Error");

        [Fact]
        public void DeleteShouldReturnUnauthorizedForUnauthorizedUsers()
            => MyController<RepliesController>
                .Instance(instance => instance
                    .WithData(TestReply)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Delete(TestReply.Id))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void DeleteShouldReturnRedirectWithValidId()
            => MyController<RepliesController>
                .Instance(controller => controller
                    .WithData(TestReply)
                    .WithData(TestComment)
                    .WithUser(user => user
                        .InRole(AdministratorRoleName)))
                .Calling(c => c.Delete(TestReply.Id))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Reply>(replies => replies
                        .All(r => r.Content != TestReply.Content
                            && r.CommentId == TestComment.Id)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ArticlesController>(c =>
                        c.Details(With.Any<int>())));
    }
}
