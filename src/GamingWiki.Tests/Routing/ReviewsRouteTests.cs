using GamingWiki.Web.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace GamingWiki.Tests.Routing
{
    public class ReviewsRouteTests
    {
        [Fact]
        public void AllShouldBeMappedWithNoPageParameter()
            => MyRouting
                .Configuration()
                .ShouldMap("/Reviews/All")
                .To<ReviewsController>(c =>
                    c.All(With.No<int>()));

        [Fact]
        public void AllShouldBeMappedWithPageParameter()
            => MyRouting
                .Configuration()
                .ShouldMap("/Reviews/All?pageIndex=1")
                .To<ReviewsController>(c =>
                    c.All(1));

        [Fact]
        public void GetCreateShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Reviews/Create?gameId=1")
                .To<ReviewsController>(c => c.Create(1));

        ////[Fact]
        ////public void PostCreateShouldBeMapped()
        ////    => MyRouting
        ////        .Configuration()
        ////        .ShouldMap(request => request
        ////            .WithMethod(HttpMethod.Post)
        ////            .WithLocation("/Articles/Create")
        ////            .WithFormFields(new
        ////            {
        ////                TestArticleFormModel.Heading,
        ////                CategoryId = 1,
        ////                TestArticleFormModel.Content,
        ////                TestArticleFormModel.PictureUrl
        ////            }))
        ////        .To<ArticlesController>(c =>
        ////            c.Create(TestArticleFormModel))
        ////        .AndAlso()
        ////        .ToValidModelState();
        
        [Fact]
        public void GetEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Reviews/Edit?reviewId=1")
                .To<ReviewsController>(c =>
                    c.Edit(1));

        //[Fact]
        //public void PostEditShouldBeMapped()
        //    => MyRouting
        //        .Configuration()
        //        .ShouldMap(request => request
        //            .WithMethod(HttpMethod.Post)
        //            .WithLocation($"/Articles/Edit?articleId={TestValidArticleEditModel.Id}")
        //            .WithFormFields(new
        //            {
        //                TestValidArticleEditModel.Id,
        //                TestValidArticleEditModel.Heading,
        //                TestValidArticleEditModel.Content,
        //                TestValidArticleEditModel.PictureUrl
        //            }))
        //        .To<ArticlesController>(c =>
        //            c.Edit(TestValidArticleEditModel, TestValidArticleEditModel.Id))
        //        .AndAlso()
        //        .ToValidModelState();

        [Fact]
        public void DeleteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Reviews/Delete?reviewId=1")
                .To<ReviewsController>(c =>
                    c.Delete(1));

        [Fact]
        public void GetSearchShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Reviews/Search?parameter=a")
                .To<ReviewsController>(c =>
                    c.Search("a", With.No<int>(), null));

        [Fact]
        public void GetSearchShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap("/Reviews/Search?parameter=a&pageIndex=1")
            .To<ReviewsController>(c =>
                c.Search("a", 1, null));

        [Fact]
        public void GetSearchShouldBeMappedWithoutName()
            => MyRouting
                .Configuration()
                .ShouldMap("/Reviews/Search?parameter=a&pageIndex=1")
                .To<ReviewsController>(c =>
                    c.Search("a", 1, With.No<string>()));

        [Fact]
        public void GetSearchShouldBeMappedWithName()
            => MyRouting
                .Configuration()
                .ShouldMap("/Reviews/Search?parameter=a&pageIndex=1&name=test")
                .To<ReviewsController>(c =>
                    c.Search("a", 1, "test"));

        [Fact]
        public void PostSearchShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/Reviews/Search?searchCriteria=abc"))
                .To<ReviewsController>(c =>
                    c.Search("abc", With.No<int>()));

        [Fact]
        public void PostSearchShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithMethod(HttpMethod.Post)
                .WithLocation("/Reviews/Search?searchCriteria=abc&pageIndex=1"))
            .To<ReviewsController>(c =>
                c.Search("abc", 1));

        [Fact]
        public void MineShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Reviews/Mine")
                .To<ReviewsController>(c =>
                    c.Mine(With.No<int>()));

        [Fact]
        public void MineShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap("/Reviews/Mine?pageIndex=1")
            .To<ReviewsController>(c =>
                c.Mine(1));
    }
}
