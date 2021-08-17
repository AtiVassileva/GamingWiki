using GamingWiki.Models;
using GamingWiki.Web.Controllers;
using static GamingWiki.Tests.Data.Comments;
using static GamingWiki.Tests.Data.Articles;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using MyTested.AspNetCore.Mvc;
using System.Linq;
using GamingWiki.Web.Areas.Admin;
using Xunit;

namespace GamingWiki.Tests.Pipeline
{
    public class CommentsPipelineTests
    {
        [Fact]
        public void AddShouldBeMappedAndRedirectUponSuccessfulAction()
        => MyPipeline
            .Configuration()
        .ShouldMap(request => request
            .WithMethod(HttpMethod.Post)
        .WithLocation($"/Comments/Add?articleId={TestArticle.Id}")
            .WithUser()
            .WithAntiForgeryToken()
        .WithFormFields(new
        {
            TestValidCommentFormModel.Content
        }))
        .To<CommentsController>(c =>
        c.Add(TestValidCommentFormModel, TestArticle.Id))
            .Which(controller => controller
                .WithData(TestArticle))
            .ShouldHave()
            .Data(data => data
                .WithSet<Comment>(comments => comments
                    .Any(c =>
                        c.Content == TestComment.Content
                        && c.ArticleId == TestArticle.Id)
                )).TempData(tempData => tempData
                .ContainingEntryWithKey(GlobalMessageKey))
            .AndAlso()
            .ShouldReturn()
            .Redirect(redirect => redirect
                .To<ArticlesController>(c =>
                    c.Details(With.Any<int>())));

        [Fact]
        public void AddShouldBeMappedAndReturnErrorViewWithInvalidArticleId()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"/Comments/Add?articleId={TestArticle.Id}")
                    .WithUser()
                    .WithAntiForgeryToken().WithFormFields(new
                    {
                        TestValidCommentFormModel.Content
                    }))
                .To<CommentsController>(c => c
                    .Add(TestValidCommentFormModel, TestArticle.Id))
                .Which()
                .ShouldReturn()
                .View("Error");

        [Fact]
        public void AddShouldBeMappedAndRedirectUponUnsuccessfulAction()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"/Comments/Add?articleId={TestArticle.Id}")
                    .WithUser()
                    .WithAntiForgeryToken()
                    .WithFormFields(new
                    {
                        TestInvalidCommentFormModel.Content
                    }))
                .To<CommentsController>(c =>
                    c.Add(TestInvalidCommentFormModel, TestArticle.Id))
                .Which(controller => controller
                    .WithData(TestArticle))
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
        
        [Fact]
        public void DeleteShouldBeMappedAndRedirectUponSuccessfulAction()
        => MyPipeline
            .Configuration()
        .ShouldMap(request => request
        .WithLocation($"/Comments/Delete?commentId={TestComment.Id}")
            .WithUser(user => user.InRole(AdministratorRoleName))
            .WithAntiForgeryToken())
        .To<CommentsController>(c => c.Delete(TestComment.Id))
            .Which(controller => controller
                .WithData(TestComment))
            .ShouldHave()
            .Data(data => data
                .WithSet<Comment>(comments => comments
                    .All(c => c.Id != TestComment.Id)
                ))
            .TempData(tempData => tempData
                .ContainingEntryWithKey(GlobalMessageKey))
            .AndAlso()
            .ShouldReturn()
            .Redirect(redirect => redirect
                .To<ArticlesController>(c =>
                    c.Details(With.Any<int>())));

        [Fact]
        public void DeleteShouldBeMappedAndReturnErrorViewWithInvalidId()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"/Comments/Delete?commentId={TestComment.Id}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<CommentsController>(c => c
                    .Delete(TestComment.Id))
                .Which()
                .ShouldReturn()
                .View("Error");
    }
}
