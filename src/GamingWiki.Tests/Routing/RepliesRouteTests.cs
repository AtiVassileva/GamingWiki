using static GamingWiki.Web.Areas.Admin.AdminConstants;
using GamingWiki.Web.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;
using static GamingWiki.Tests.Data.Comments;
using static GamingWiki.Tests.Data.Replies;

namespace GamingWiki.Tests.Routing
{
    public class RepliesRouteTests
    {
        [Fact]
        public void AddShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"/Replies/Add?commentId={TestComment.Id}")
                    .WithFormFields(new
                    {
                        TestValidReplyFormModel.ReplyContent
                    }))
                .To<RepliesController>(c =>
                    c.Add(TestValidReplyFormModel, TestComment.Id))
                .Which(controller => controller
                    .WithData(TestComment)
                    .WithUser())
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ArticlesController>(c => c.Details(With.Any<int>())));

        [Fact]
        public void DeleteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap($"/Replies/Delete?replyId={TestReply.Id}")
                .To<RepliesController>(c =>
                    c.Delete(TestReply.Id))
                .Which(controller => controller
                    .WithData(TestReply)
                    .WithData(TestComment)
                    .WithUser(user => user.InRole(AdministratorRoleName)))
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ArticlesController>(c => c.Details(With.Any<int>())));
    }
}
