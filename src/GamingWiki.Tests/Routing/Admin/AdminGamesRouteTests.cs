using GamingWiki.Web.Areas.Admin.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace GamingWiki.Tests.Routing.Admin
{
    public class AdminGamesRouteTests
    {
        [Fact]
        public void ApproveShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Games/Approve?gameId=1")
                .To<GamesController>(c =>
                    c.Approve(1));

        [Fact]
        public void PendingShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Games/Pending")
                .To<GamesController>(c => 
                    c.Pending());
    }
}
