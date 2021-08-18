using GamingWiki.Web.Controllers;
using static GamingWiki.Tests.Data.Messages;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace GamingWiki.Tests.Routing
{
    public class MessagesRouteTests
    {
        [Fact]
        public void DeleteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap($"/Messages/Delete?messageId={TestMessage.Id}")
                .To<MessagesController>(c =>
                    c.Delete(TestMessage.Id))
                .Which(controller => controller
                    .WithData(TestMessage))
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<DiscussionsController>(c => c.Chat(With.Any<int>())));
    }
}
