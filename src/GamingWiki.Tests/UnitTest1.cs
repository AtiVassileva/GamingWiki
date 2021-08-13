using GamingWiki.Models;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Web.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace GamingWiki.Tests
{
    public class UnitTest1
    {
        private const int DefaultPageIndex = 1;

        [Fact]
        public void GetAllShouldReturnView()
            => MyController<ArticlesController>
                .Instance()
                .Calling(a => a.All(DefaultPageIndex))
                .ShouldReturn()
                .View();
        
        [Fact]
        public void GetCreateShouldReturnView()
            => MyController<ArticlesController>
                .Instance()
                .Calling(a => a.Create())
                .ShouldReturn()
                .View();

        [Fact]
        public void GetDetailsShouldReturnDetailsViewWithValidArticleId()
            => MyMvc
                .Pipeline()
                .ShouldMap("/Articles/Details")
                .To<ArticlesController>(a => a.Details(17))
                .Which(controller => controller
                    .WithData(new Article
                    {
                        Id = 17
                    }))
                .ShouldReturn()
                .View(view => view.WithModelOfType<ArticleServiceDetailsModel>());

        [Fact]
        public void GetDetailsShouldReturnErrorViewWithInvalidArticleId()
            => MyController<ArticlesController>
                .Instance()
                .Calling(a => a.Details(-3))
                .ShouldReturn()
                .View("Error");
    }
}
