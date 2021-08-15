using System.Linq;
using GamingWiki.Models;
using GamingWiki.Web.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;
using static GamingWiki.Tests.Data.Messages;
using static GamingWiki.Tests.Data.Discussions;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using static GamingWiki.Web.Common.WebConstants;

namespace GamingWiki.Tests.Controllers
{
    public class MessagesControllerTests
    {
        [Fact]
        public void DeleteShouldReturnErrorViewWithInvalidId()
            => MyController<MessagesController>
                .Instance()
                .Calling(c => c.Delete(TestMessage.Id))
                .ShouldReturn()
                .View("Error");

        [Fact]
        public void DeleteShouldReturnUnauthorizedForUnauthorizedUsers()
            => MyController<MessagesController>
                .Instance(controller => controller
                    .WithData(TestMessage)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Delete(TestMessage.Id))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void DeleteShouldRedirectWithValidId()
            => MyController<MessagesController>
                .Instance(controller => controller
                    .WithData(TestDiscussion)
                    .WithData(TestMessage)
                    .WithUser(user => user.InRole(AdministratorRoleName)))
                .Calling(c => c.Delete(TestMessage.Id))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Message>(messages => messages
                        .All(m => m.Id != TestMessage.Id)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<DiscussionsController>(c => c
                        .Chat(With.Any<int>())));

    }
}