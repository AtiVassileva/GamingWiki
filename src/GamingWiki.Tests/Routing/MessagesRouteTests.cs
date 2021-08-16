using GamingWiki.Web.Controllers;
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
                .ShouldMap("/Messages/Delete?messageId=1")
                .To<MessagesController>(c =>
                    c.Delete(1));
    }
}
