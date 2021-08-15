using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Home;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace GamingWiki.Tests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void IndexShouldReturnGuestPageViewForUnauthenticatedUser()
            => MyController<HomeController>
                .Instance()
                .Calling(c => c.Index())
                .ShouldReturn()
                .View("GuestPage");

        [Fact]
        public void IndexShouldReturnIndexViewForAuthenticatedUser()
        => MyController<HomeController>
            .Instance(controller => controller
                .WithUser(TestUser.Identifier))
            .Calling(c => c.Index())
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<HomeViewModel>());

        [Fact]
        public void AboutShouldReturnView()
        => MyController<HomeController>
            .Instance()
            .Calling(c => c.About())
            .ShouldReturn()
            .View();

        [Fact]
        public void ErrorShouldReturnIndexView()
            => MyController<HomeController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Error())
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>());
    }
}
