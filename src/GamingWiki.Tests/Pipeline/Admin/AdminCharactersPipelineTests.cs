using System.Collections.Generic;
using System.Linq;
using GamingWiki.Models;
using GamingWiki.Services.Models.Characters;
using static GamingWiki.Tests.Data.Characters;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using GamingWiki.Web.Areas.Admin.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace GamingWiki.Tests.Pipeline.Admin
{
    public class AdminCharactersPipelineTests
    {
        [Fact]
        public void ApproveShouldBeMappedAndReturnCorrectView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Admin/Characters/Approve?CharacterId={TestNotApprovedCharacter.Id}")
                    .WithUser(user => user.InRole(AdministratorRoleName))
                    .WithAntiForgeryToken())
                .To<CharactersController>(c => c.Approve(TestNotApprovedCharacter.Id))
                .Which(controller => controller
                    .WithData(TestNotApprovedCharacter))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Character>(characters => characters
                        .Any(g => g.Id == TestNotApprovedCharacter.Id
                                  && g.IsApproved == true)))
                .AndAlso()
                .ShouldReturn()
                .Redirect();

        [Fact]
        public void PendingShouldBeMappedAndReturnCorrectView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation("/Admin/Characters/Pending")
                    .WithUser(user => user.InRole(AdministratorRoleName))
                    .WithAntiForgeryToken())
                .To<CharactersController>(c => c.Pending())
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<IEnumerable<CharacterPendingModel>>());
    }
}
