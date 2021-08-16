using GamingWiki.Models;
using GamingWiki.Web.Areas.Admin.Controllers;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using static GamingWiki.Tests.Data.Games;
using MyTested.AspNetCore.Mvc;
using Xunit;
using System.Linq;

namespace GamingWiki.Tests.Controllers.Admin
{
    public class AdminGamesControllerTests
    {
        [Fact]
        public void ApproveShouldWorkCorrectlyAndRedirect()
        => MyController<GamesController>
            .Instance(controller => controller
                .WithData(TestNotApprovedGame)
                .WithUser(user => user
                    .InRole(AdministratorRoleName)))
            .Calling(c => c.Approve(TestNotApprovedGame.Id))
            .ShouldHave()
            .Data(data => data
                .WithSet<Game>(games => games
                    .Any(g =>
                        g.Id == TestNotApprovedGame.Id &&
                        g.IsApproved == true)))
            .AndAlso()
            .ShouldReturn()
            .Redirect(redirect => redirect
                .To<Web.Controllers.GamesController>(c =>
                    c.All(With.Any<int>())));

        [Fact]
        public void PendingShouldReturnView()
            => MyController<GamesController>
                .Instance()
                .Calling(c => c.Pending())
                .ShouldReturn()
                .View();
    }
}
