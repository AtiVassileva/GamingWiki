using GamingWiki.Models;
using GamingWiki.Web.Controllers;
using static GamingWiki.Tests.Data.Messages;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Common.ExceptionMessages;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using MyTested.AspNetCore.Mvc;
using System.Linq;
using GamingWiki.Web.Models;
using Xunit;

namespace GamingWiki.Tests.Pipeline
{
    public class MessagesPipelineTests
    {
        [Fact]
        public void DeleteShouldBeMappedAndRedirectUponSuccessfulAction()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Messages/Delete?messageId={TestMessage.Id}")
                    .WithUser(user => user.InRole(AdministratorRoleName))
                    .WithAntiForgeryToken())
                .To<MessagesController>(c => c.Delete(TestMessage.Id))
                .Which(controller => controller
                    .WithData(TestMessage))
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

        [Fact]
        public void DeleteShouldBeMappedAndReturnErrorViewWithInvalidId()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Messages/Delete?messageId={TestMessage.Id}")
                    .WithUser(user => user.InRole(AdministratorRoleName))
                    .WithAntiForgeryToken())
                .To<MessagesController>(c => c.Delete(TestMessage.Id))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>(m => 
                        m.Message == NonExistingMessageExceptionMessage));

        [Fact]
        public void DeleteShouldBeMappedAndReturnUnauthorizedForUnauthorizedUsers()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Messages/Delete?messageId={TestMessage.Id}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<MessagesController>(c => c.Delete(TestMessage.Id))
                .Which(controller => controller
                    .WithData(TestMessage))
                .ShouldReturn()
                .Unauthorized();
    }
}
