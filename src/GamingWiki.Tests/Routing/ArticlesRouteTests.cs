using GamingWiki.Services.Models.Articles;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models.Articles;
using MyTested.AspNetCore.Mvc;
using Xunit;
using static GamingWiki.Tests.Data.Articles;

namespace GamingWiki.Tests.Routing
{
    public class ArticlesRouteTests
    {
        [Fact]
        public void AllShouldBeMappedWithNoPageParameter()
            => MyRouting
                .Configuration()
                .ShouldMap("/Articles/All")
                .To<ArticlesController>(c =>
                    c.All(With.No<int>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFullModel>());

        [Fact]
        public void AllShouldBeMappedWithPageParameter()
            => MyRouting
                .Configuration()
                .ShouldMap("/Articles/All?pageIndex=1")
                .To<ArticlesController>(c =>
                    c.All(1))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFullModel>());

        [Fact]
        public void GetCreateShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Articles/Create")
                .To<ArticlesController>(c => c.Create())
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFormModel>());

        [Fact]
        public void PostCreateShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/Articles/Create"))
                .To<ArticlesController>(c =>
                    c.Create(With.Any<ArticleFormModel>()));

        [Fact]
        public void DetailsShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap($"/Articles/Details?articleId={TestArticle.Id}")
                .To<ArticlesController>(c =>
                    c.Details(TestArticle.Id))
                .Which(controller => controller
                    .WithData(TestArticle))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleServiceDetailsModel>());

        [Fact]
        public void GetEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap($"/Articles/Edit?articleId={TestArticle.Id}")
                .To<ArticlesController>(c =>
                    c.Edit(TestArticle.Id))
                .Which(controller => controller
                    .WithData(TestArticle)
                    .WithUser(user => user.InRole(AdministratorRoleName)))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleServiceEditModel>());

        [Fact]
        public void PostEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"/Articles/Edit?articleId={TestValidArticleEditModel.Id}")
                    .WithFormFields(new
                    {
                        TestValidArticleEditModel.Id,
                        TestValidArticleEditModel.Heading,
                        TestValidArticleEditModel.Content,
                        TestValidArticleEditModel.PictureUrl
                    }))
                .To<ArticlesController>(c =>
                    c.Edit(TestValidArticleEditModel, TestValidArticleEditModel.Id))
                .AndAlso()
                .ToValidModelState();

        [Fact]
        public void DeleteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Articles/Delete?articleId=1")
                .To<ArticlesController>(c =>
                    c.Delete(1));

        [Fact]
        public void GetSearchShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Articles/Search?parameter=a")
                .To<ArticlesController>(c =>
                    c.Search("a", With.No<int>(), null))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFullModel>());

        [Fact]
        public void GetSearchShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap("/Articles/Search?parameter=a&pageIndex=1")
            .To<ArticlesController>(c =>
                c.Search("a", 1, null))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ArticleFullModel>());

        [Fact]
        public void GetSearchShouldBeMappedWithoutName()
            => MyRouting
                .Configuration()
                .ShouldMap("/Articles/Search?parameter=a&pageIndex=1")
                .To<ArticlesController>(c =>
                    c.Search("a", 1, With.No<string>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFullModel>());

        [Fact]
        public void GetSearchShouldBeMappedWithName()
            => MyRouting
                .Configuration()
                .ShouldMap("/Articles/Search?parameter=a&pageIndex=1&name=test")
                .To<ArticlesController>(c =>
                    c.Search("a", 1, "test"))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFullModel>());

        [Fact]
        public void PostSearchShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/Articles/Search?searchCriteria=abc"))
                .To<ArticlesController>(c =>
                    c.Search("abc", With.No<int>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFullModel>());

        [Fact]
        public void PostSearchShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithMethod(HttpMethod.Post)
                .WithLocation("/Articles/Search?searchCriteria=abc&pageIndex=1"))
            .To<ArticlesController>(c =>
                c.Search("abc", 1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ArticleFullModel>());

        [Fact]
        public void FilterShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Articles/Filter?parameter=1")
                .To<ArticlesController>(c =>
                    c.Filter(1, With.No<int>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFullModel>());

        [Fact]
        public void FilterShouldBeMappedWithPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Articles/Filter?parameter=1&pageIndex=1")
                .To<ArticlesController>(c =>
                    c.Filter(1, 1))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFullModel>());


        [Fact]
        public void MineShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Articles/Mine")
                .To<ArticlesController>(c =>
                    c.Mine(With.No<int>()))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFullModel>());

        [Fact]
        public void MineShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap("/Articles/Mine?pageIndex=1")
            .To<ArticlesController>(c =>
                c.Mine(1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ArticleFullModel>());

    }
}
