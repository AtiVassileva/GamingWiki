using GamingWiki.Services.Models.Discussions;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using static GamingWiki.Tests.Data.Discussions;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models.Discussions;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace GamingWiki.Tests.Routing
{
    public class DiscussionsRouteTests
    {
        [Fact]
        public void AllShouldBeMappedWithNoPageParameter()
            => MyRouting
                .Configuration()
                .ShouldMap("/Discussions/All")
                .To<DiscussionsController>(c =>
                    c.All(With.No<int>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionFullModel>());

        [Fact]
        public void AllShouldBeMappedWithPageParameter()
            => MyRouting
                .Configuration()
                .ShouldMap("/Discussions/All?pageIndex=1")
                .To<DiscussionsController>(c =>
                    c.All(1))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionFullModel>());

        [Fact]
        public void GetCreateShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Discussions/Create")
                .To<DiscussionsController>(c => c.Create())
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionFormModel>());

        [Fact]
        public void PostCreateShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/Discussions/Create"))
                .To<DiscussionsController>(c =>
                    c.Create(With.Any<DiscussionFormModel>()));

        [Fact]
        public void DetailsShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap($"/Discussions/Details?discussionId={TestDiscussion.Id}")
                .To<DiscussionsController>(c =>
                    c.Details(TestDiscussion.Id))
                .Which(controller => controller
                    .WithData(TestDiscussion))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionServiceDetailsModel>());

        [Fact]
        public void GetEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap($"/Discussions/Edit?discussionId={TestDiscussion.Id}")
                .To<DiscussionsController>(c =>
                    c.Edit(TestDiscussion.Id))
                .Which(controller => controller
                    .WithData(TestDiscussion)
                    .WithUser(user => user.InRole(AdministratorRoleName)))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionServiceEditModel>());

        [Fact]
        public void PostEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"/Discussions/Edit?discussionId={TestDiscussion.Id}"))
                .To<DiscussionsController>(c =>
                    c.Edit(With.Any<DiscussionServiceEditModel>(),
                        TestDiscussion.Id));

        [Fact]
        public void DeleteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Discussions/Delete?discussionId=1")
                .To<DiscussionsController>(c =>
                    c.Delete(1));

        [Fact]
        public void GetSearchShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Discussions/Search?parameter=a")
                .To<DiscussionsController>(c =>
                    c.Search("a", With.No<int>(), null))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionFullModel>());

        [Fact]
        public void GetSearchShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap("/Discussions/Search?parameter=a&pageIndex=1")
            .To<DiscussionsController>(c =>
                c.Search("a", 1, null))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<DiscussionFullModel>());

        [Fact]
        public void GetSearchShouldBeMappedWithoutName()
            => MyRouting
                .Configuration()
                .ShouldMap("/Discussions/Search?parameter=a&pageIndex=1")
                .To<DiscussionsController>(c =>
                    c.Search("a", 1, With.No<string>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionFullModel>());

        [Fact]
        public void GetSearchShouldBeMappedWithName()
            => MyRouting
                .Configuration()
                .ShouldMap("/Discussions/Search?parameter=a&pageIndex=1&name=test")
                .To<DiscussionsController>(c =>
                    c.Search("a", 1, "test"))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionFullModel>());

        [Fact]
        public void PostSearchShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/Discussions/Search?searchCriteria=abc"))
                .To<DiscussionsController>(c =>
                    c.Search("abc", With.No<int>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionFullModel>());

        [Fact]
        public void PostSearchShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithMethod(HttpMethod.Post)
                .WithLocation("/Discussions/Search?searchCriteria=abc&pageIndex=1"))
            .To<DiscussionsController>(c =>
                c.Search("abc", 1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<DiscussionFullModel>());

        [Fact]
        public void JoinShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Discussions/Join?discussionId=1")
                .To<DiscussionsController>(c =>
                    c.Join(1));

        [Fact]
        public void LeaveShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Discussions/Leave?discussionId=1")
                .To<DiscussionsController>(c =>
                    c.Leave(1));

        [Fact]
        public void ChatShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap($"/Discussions/Chat?discussionId={TestDiscussion.Id}")
                .To<DiscussionsController>(c =>
                   c.Chat(TestDiscussion.Id))
                .Which(controller => controller
                    .WithData(TestDiscussion)
                    .WithData(TestUserDiscussionWithTestUser)
                    .WithUser())
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionChatServiceModel>());

        [Fact]
        public void MineShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Discussions/Mine")
                .To<DiscussionsController>(c =>
                    c.Mine(With.No<int>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<DiscussionFullModel>());

        [Fact]
        public void MineShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap("/Discussions/Mine?pageIndex=1")
            .To<DiscussionsController>(c =>
                c.Mine(1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<DiscussionFullModel>());
    }
}
