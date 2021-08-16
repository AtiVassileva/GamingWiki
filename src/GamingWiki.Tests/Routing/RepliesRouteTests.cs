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
                .AndAlso()
                .ToValidModelState();

        [Fact]
        public void DeleteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Replies/Delete?replyId=1")
                .To<RepliesController>(c =>
                    c.Delete(1));
    }
}
