using System;
using GamingWiki.Models;
using GamingWiki.Web.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;
using static GamingWiki.Tests.Data.Articles;
using static GamingWiki.Tests.Data.Comments;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Areas.Admin.AdminConstants;

using System.Linq;

namespace GamingWiki.Tests.Controllers
{
    public class CommentsControllerTests
    {
        [Fact]
        public void CommentsControllerShouldBeForAuthorizedUsersOnly()
            => MyController<CommentsController>
                .ShouldHave()
                .Attributes(attributes => attributes
                    .RestrictingForAuthorizedRequests());

        [Fact]
        public void AddShouldReturnRedirectWithValidModel()
            => MyController<CommentsController>
                .Instance(controller => controller
                    .WithData(TestArticle))
                .Calling(c => c.Add(TestValidCommentFormModel, TestArticle.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                    .WithSet<Comment>(comments =>
                        comments.Any(c =>
                            c.Content == TestValidCommentFormModel.Content
                            && c.ArticleId == TestArticle.Id)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ArticlesController>
                        (c => c.Details(With.Any<int>())));

        [Fact]
        public void AddShouldRedirectWithInvalidModelState()
            => MyController<CommentsController>
                .Instance(controller => controller
                    .WithData(TestArticle)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Add(TestInvalidCommentFormModel, TestArticle.Id))
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
        public void AddShouldReturnErrorViewWithInvalidArticleId()
            => MyController<CommentsController>
                .Instance()
                .Calling(c => c.Add(TestValidCommentFormModel, 
                    TestComment.Id))
                .ShouldReturn()
                .View("Error");

        [Fact]
        public void DeleteShouldReturnErrorViewWithInvalidCommentId()
            => MyController<CommentsController>
                .Instance()
                .Calling(c => c.Delete(new Random().Next()))
                .ShouldReturn()
                .View("Error");

        [Fact]
        public void DeleteShouldReturnUnauthorizedForUnauthorizedUsers()
            => MyController<CommentsController>
                .Instance(controller => controller
                    .WithData(TestComment))
                .Calling(c => c.Delete(TestComment.Id))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void DeleteShouldReturnRedirectWithValidId()
            => MyController<CommentsController>
                .Instance(controller => controller
                    .WithData(TestComment)
                    .WithData(TestArticle)
                    .WithUser(user => user
                        .InRole(AdministratorRoleName)))
                .Calling(c => c.Delete(TestComment.Id))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Comment>(comments => comments
                        .All(c =>
                            c.Content != TestComment.Content
                            && c.ArticleId != TestArticle.Id)
                    )).TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ArticlesController>(c =>
                        c.Details(With.Any<int>())));

    }
}
