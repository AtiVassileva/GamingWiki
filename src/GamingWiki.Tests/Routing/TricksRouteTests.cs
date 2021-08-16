using GamingWiki.Web.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace GamingWiki.Tests.Routing
{
    public class TricksRouteTests
    {
        [Fact]
        public void AllShouldBeMappedWithNoPageParameter()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tricks/All")
                .To<TricksController>(c =>
                    c.All(With.No<int>()));

        [Fact]
        public void AllShouldBeMappedWithPageParameter()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tricks/All?pageIndex=1")
                .To<TricksController>(c =>
                    c.All(1));

        [Fact]
        public void GetCreateShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tricks/Create")
                .To<TricksController>(c => c.Create());

        //////[Fact]
        //////public void PostCreateShouldBeMapped()
        //////    => MyRouting
        //////        .Configuration()
        //////        .ShouldMap(request => request
        //////            .WithMethod(HttpMethod.Post)
        //////            .WithLocation("/Articles/Create")
        //////            .WithFormFields(new
        //////            {
        //////                TestArticleFormModel.Heading,
        //////                CategoryId = 1,
        //////                TestArticleFormModel.Content,
        //////                TestArticleFormModel.PictureUrl
        //////            }))
        //////        .To<ArticlesController>(c =>
        //////            c.Create(TestArticleFormModel))
        //////        .AndAlso()
        //////        .ToValidModelState();

        [Fact]
        public void GetEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tricks/Edit?trickId=1")
                .To<TricksController>(c =>
                    c.Edit(1));

        ////[Fact]
        ////public void PostEditShouldBeMapped()
        ////    => MyRouting
        ////        .Configuration()
        ////        .ShouldMap(request => request
        ////            .WithMethod(HttpMethod.Post)
        ////            .WithLocation($"/Articles/Edit?articleId={TestValidArticleEditModel.Id}")
        ////            .WithFormFields(new
        ////            {
        ////                TestValidArticleEditModel.Id,
        ////                TestValidArticleEditModel.Heading,
        ////                TestValidArticleEditModel.Content,
        ////                TestValidArticleEditModel.PictureUrl
        ////            }))
        ////        .To<ArticlesController>(c =>
        ////            c.Edit(TestValidArticleEditModel, TestValidArticleEditModel.Id))
        ////        .AndAlso()
        ////        .ToValidModelState();

        [Fact]
        public void DeleteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tricks/Delete?trickId=1")
                .To<TricksController>(c =>
                    c.Delete(1));

        [Fact]
        public void GetSearchShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tricks/Search?parameter=a")
                .To<TricksController>(c =>
                    c.Search("a", With.No<int>(), null));

        [Fact]
        public void GetSearchShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap("/Tricks/Search?parameter=a&pageIndex=1")
            .To<TricksController>(c =>
                c.Search("a", 1, null));

        [Fact]
        public void GetSearchShouldBeMappedWithoutName()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tricks/Search?parameter=a&pageIndex=1")
                .To<TricksController>(c =>
                    c.Search("a", 1, With.No<string>()));

        [Fact]
        public void GetSearchShouldBeMappedWithName()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tricks/Search?parameter=a&pageIndex=1&name=test")
                .To<TricksController>(c =>
                    c.Search("a", 1, "test"));

        [Fact]
        public void PostSearchShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/Tricks/Search?searchCriteria=abc"))
                .To<TricksController>(c =>
                    c.Search("abc", With.No<int>()));

        [Fact]
        public void PostSearchShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithMethod(HttpMethod.Post)
                .WithLocation("/Tricks/Search?searchCriteria=abc&pageIndex=1"))
            .To<TricksController>(c =>
                c.Search("abc", 1));

        [Fact]
        public void MineShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tricks/Mine")
                .To<TricksController>(c =>
                    c.Mine(With.No<int>()));

        [Fact]
        public void MineShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap("/Tricks/Mine?pageIndex=1")
            .To<TricksController>(c =>
                c.Mine(1));
    }
}
