using GamingWiki.Web.Controllers;
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
                    c.All(With.No<int>()));

        [Fact]
        public void AllShouldBeMappedWithPageParameter()
            => MyRouting
                .Configuration()
                .ShouldMap("/Articles/All?pageIndex=1")
                .To<ArticlesController>(c =>
                    c.All(1));

        [Fact]
        public void GetCreateShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Articles/Create")
                .To<ArticlesController>(c => c.Create());

        //[Fact]
        //public void PostCreateShouldBeMapped()
        //    => MyRouting
        //        .Configuration()
        //        .ShouldMap(request => request
        //            .WithMethod(HttpMethod.Post)
        //            .WithLocation("/Articles/Create")
        //            .WithFormFields(new
        //            {
        //                TestArticleFormModel.Heading,
        //                CategoryId = 1,
        //                TestArticleFormModel.Content,
        //                TestArticleFormModel.PictureUrl
        //            }))
        //        .To<ArticlesController>(c =>
        //            c.Create(TestArticleFormModel))
        //        .AndAlso()
        //        .ToValidModelState();

        [Fact]
        public void DetailsShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Articles/Details?articleId=1")
                .To<ArticlesController>(c =>
                    c.Details(1));

        [Fact]
        public void GetEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Articles/Edit?articleId=1")
                .To<ArticlesController>(c =>
                    c.Edit(1));
        
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
                    c.Search("a", With.No<int>(), null));

        [Fact]
        public void GetSearchShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap("/Articles/Search?parameter=a&pageIndex=1")
            .To<ArticlesController>(c =>
                c.Search("a", 1, null));

        [Fact]
        public void GetSearchShouldBeMappedWithoutName()
            => MyRouting
                .Configuration()
                .ShouldMap("/Articles/Search?parameter=a&pageIndex=1")
                .To<ArticlesController>(c =>
                    c.Search("a", 1, With.No<string>()));
        
        [Fact]
        public void GetSearchShouldBeMappedWithName()
            => MyRouting
                .Configuration()
                .ShouldMap("/Articles/Search?parameter=a&pageIndex=1&name=test")
                .To<ArticlesController>(c =>
                    c.Search("a", 1, "test"));

        [Fact]
        public void PostSearchShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/Articles/Search?searchCriteria=abc"))
                .To<ArticlesController>(c =>
                    c.Search("abc", With.No<int>()));

        [Fact]
        public void PostSearchShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithMethod(HttpMethod.Post)
                .WithLocation("/Articles/Search?searchCriteria=abc&pageIndex=1"))
            .To<ArticlesController>(c =>
                c.Search("abc", 1));

        [Fact]
        public void FilterShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Articles/Filter?parameter=1")
                .To<ArticlesController>(c =>
                    c.Filter(1, With.No<int>()));

        [Fact]
        public void FilterShouldBeMappedWithPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Articles/Filter?parameter=1&pageIndex=1")
                .To<ArticlesController>(c =>
                    c.Filter(1, 1));


        [Fact]
        public void MineShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Articles/Mine")
                .To<ArticlesController>(c =>
                    c.Mine(With.No<int>()));

        [Fact]
        public void MineShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap("/Articles/Mine?pageIndex=1")
            .To<ArticlesController>(c =>
                c.Mine(1));

    }
}
