using System.Collections.Generic;
using GamingWiki.Models;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using static GamingWiki.Tests.Data.Games;
using MyTested.AspNetCore.Mvc;
using System.Linq;
using GamingWiki.Services.Models.Games;
using GamingWiki.Web.Areas.Admin.Controllers;
using Xunit;

namespace GamingWiki.Tests.Pipeline.Admin
{
    public class AdminGamesPipelineTests
    {
        [Fact]
        public void ApproveShouldBeMappedAndReturnCorrectView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Admin/Games/Approve?gameId={TestNotApprovedGame.Id}")
                    .WithUser(user => user.InRole(AdministratorRoleName))
                    .WithAntiForgeryToken())
                .To<GamesController>(c => c.Approve(TestNotApprovedGame.Id))
                .Which(controller => controller
                    .WithData(TestNotApprovedGame))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Game>(games => games
                        .Any(g => g.Id == TestNotApprovedGame.Id
                                  && g.IsApproved == true)))
                .AndAlso()
                .ShouldReturn()
                .Redirect();

        [Fact]
        public void PendingShouldBeMappedAndReturnCorrectView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation("/Admin/Games/Pending")
                    .WithUser(user => user.InRole(AdministratorRoleName))
                    .WithAntiForgeryToken())
                .To<GamesController>(c => c.Pending())
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<IEnumerable<GamePendingModel>>());
    }
}
