using System.Collections.Generic;
using System.Linq;
using GamingWiki.Models;
using GamingWiki.Services.Models.Characters;
using GamingWiki.Web.Areas.Admin.Controllers;
using static GamingWiki.Tests.Data.Characters;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace GamingWiki.Tests.Controllers.Admin
{
    public class AdminCharactersControllerTests
    {
        [Fact]
        public void ApproveShouldWorkCorrectlyAndRedirect()
            => MyController<CharactersController>
                .Instance(controller => controller
                    .WithData(TestNotApprovedCharacter)
                    .WithUser(user => user
                        .InRole(AdministratorRoleName)))
                .Calling(c => c.Approve(TestNotApprovedCharacter.Id))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Character>(characters => characters
                        .Any(g => g.Id == TestNotApprovedCharacter.Id &&
                                  g.IsApproved == true)))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<Web.Controllers.CharactersController>(c =>
                        c.All(With.Any<int>())));

        [Fact]
        public void PendingShouldReturnView()
            => MyController<CharactersController>
                .Instance()
                .Calling(c => c.Pending())
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<IEnumerable<CharacterPendingModel>>());
    }
}
