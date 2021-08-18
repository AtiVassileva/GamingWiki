using System.Linq;
using GamingWiki.Models;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Common.ExceptionMessages;
using static GamingWiki.Tests.Data.Replies;
using static GamingWiki.Tests.Data.Comments;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace GamingWiki.Tests.Pipeline
{
    public class RepliesPipelineTests
    {
        [Fact]
        public void AddShouldBeMappedAndRedirectUponSuccessfulAction()
        => MyPipeline
            .Configuration()
        .ShouldMap(request => request
            .WithMethod(HttpMethod.Post)
        .WithLocation($"/Replies/Add?commentId={TestComment.Id}")
            .WithUser()
            .WithAntiForgeryToken()
        .WithFormFields(new
        {
            TestValidReplyFormModel.ReplyContent
        }))
        .To<RepliesController>(c =>
        c.Add(TestValidReplyFormModel, TestComment.Id))
            .Which(controller => controller
                .WithData(TestComment))
            .ShouldHave()
            .Data(data => data
                .WithSet<Reply>(replies => replies
                    .Any(c =>
                        c.Content == TestValidReplyFormModel.ReplyContent
                        && c.CommentId == TestComment.Id)
                )).TempData(tempData => tempData
                .ContainingEntryWithKey(GlobalMessageKey))
            .AndAlso()
            .ShouldReturn()
            .Redirect(redirect => redirect
                .To<ArticlesController>(c =>
                    c.Details(With.Any<int>())));

        [Fact]
        public void AddShouldBeMappedAndReturnErrorViewWithInvalidCommentId()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"/Replies/Add?commentId={TestComment.Id}")
                    .WithUser()
                    .WithAntiForgeryToken().WithFormFields(new
                    {
                        TestValidReplyFormModel.ReplyContent
                    }))
                .To<RepliesController>(c => c
                    .Add(TestValidReplyFormModel, TestComment.Id))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>(m => 
                        m.Message == NonExistingCommentExceptionMessage));

        [Fact]
        public void AddShouldBeMappedAndRedirectUponUnsuccessfulAction()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"/Replies/Add?commentId={TestComment.Id}")
                    .WithUser()
                    .WithAntiForgeryToken()
                    .WithFormFields(new
                    {
                        TestInvalidReplyFormModel.ReplyContent
                    }))
                .To<RepliesController>(c =>
                    c.Add(TestInvalidReplyFormModel, TestComment.Id))
                .Which(controller => controller
                    .WithData(TestComment))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Reply>(replies => replies
                        .All(c =>
                            c.Content != TestReply.Content
                            && c.CommentId != TestComment.Id)
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
        .WithLocation($"/Replies/Delete?replyId={TestReply.Id}")
            .WithUser(user => user.InRole(AdministratorRoleName))
            .WithAntiForgeryToken())
        .To<RepliesController>(c => c.Delete(TestReply.Id))
            .Which(controller => controller
                .WithData(TestReply)
                .WithData(TestComment))
            .ShouldHave()
            .Data(data => data
                .WithSet<Reply>(replies => replies
                    .All(c => c.Id != TestReply.Id)
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
                    .WithLocation($"/Replies/Delete?replyId={TestReply.Id}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<RepliesController>(c => c
                    .Delete(TestReply.Id))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>(m =>
                        m.Message == NonExistingReplyExceptionMessage));
    }
}
