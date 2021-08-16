using GamingWiki.Web.Areas.Admin.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace GamingWiki.Tests.Routing.Admin
{
    public class AdminCharactersRouteTests
    {
        [Fact]
        public void ApproveShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Characters/Approve?characterId=1")
                .To<CharactersController>(c =>
                    c.Approve(1));

        [Fact]
        public void PendingShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Admin/Characters/Pending")
                .To<CharactersController>(c =>
                    c.Pending());
    }
}
