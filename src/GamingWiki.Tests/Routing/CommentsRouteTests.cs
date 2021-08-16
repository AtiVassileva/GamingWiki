using GamingWiki.Web.Controllers;
using static GamingWiki.Tests.Data.Comments;
using static GamingWiki.Tests.Data.Articles;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace GamingWiki.Tests.Routing
{
    public class CommentsRouteTests
    {
        [Fact]
        public void AddShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"/Comments/Add?articleId={TestArticle.Id}")
                    .WithFormFields(new
                    {
                        TestValidCommentFormModel.Content
                    }))
                .To<CommentsController>(c =>
                    c.Add(TestValidCommentFormModel, TestArticle.Id))
                .AndAlso()
                .ToValidModelState();

        [Fact]
        public void DeleteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Comments/Delete?commentId=1")
                .To<CommentsController>(c =>
                    c.Delete(1));
    }
}
