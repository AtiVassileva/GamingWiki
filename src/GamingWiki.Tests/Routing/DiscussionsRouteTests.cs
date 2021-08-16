using GamingWiki.Web.Controllers;
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
                    c.All(With.No<int>()));

        [Fact]
        public void AllShouldBeMappedWithPageParameter()
            => MyRouting
                .Configuration()
                .ShouldMap("/Discussions/All?pageIndex=1")
                .To<DiscussionsController>(c =>
                    c.All(1));

        [Fact]
        public void GetCreateShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Discussions/Create")
                .To<DiscussionsController>(c => c.Create());

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
        public void DetailsShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Discussions/Details?discussionId=1")
                .To<DiscussionsController>(c =>
                    c.Details(1));

        [Fact]
        public void GetEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Discussions/Edit?discussionId=1")
                .To<DiscussionsController>(c =>
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
                .ShouldMap("/Discussions/Delete?discussionId=1")
                .To<DiscussionsController>(c =>
                    c.Delete(1));

        [Fact]
        public void GetSearchShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Discussions/Search?parameter=a")
                .To<DiscussionsController>(c =>
                    c.Search("a", With.No<int>(), null));

        [Fact]
        public void GetSearchShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap("/Discussions/Search?parameter=a&pageIndex=1")
            .To<DiscussionsController>(c =>
                c.Search("a", 1, null));

        [Fact]
        public void GetSearchShouldBeMappedWithoutName()
            => MyRouting
                .Configuration()
                .ShouldMap("/Discussions/Search?parameter=a&pageIndex=1")
                .To<DiscussionsController>(c =>
                    c.Search("a", 1, With.No<string>()));

        [Fact]
        public void GetSearchShouldBeMappedWithName()
            => MyRouting
                .Configuration()
                .ShouldMap("/Discussions/Search?parameter=a&pageIndex=1&name=test")
                .To<DiscussionsController>(c =>
                    c.Search("a", 1, "test"));

        [Fact]
        public void PostSearchShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/Discussions/Search?searchCriteria=abc"))
                .To<DiscussionsController>(c =>
                    c.Search("abc", With.No<int>()));

        [Fact]
        public void PostSearchShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithMethod(HttpMethod.Post)
                .WithLocation("/Discussions/Search?searchCriteria=abc&pageIndex=1"))
            .To<DiscussionsController>(c =>
                c.Search("abc", 1));

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
                .ShouldMap("/Discussions/Chat?discussionId=1")
                .To<DiscussionsController>(c =>
                   c.Chat(1));

        [Fact]
        public void MineShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Discussions/Mine")
                .To<DiscussionsController>(c =>
                    c.Mine(With.No<int>()));

        [Fact]
        public void MineShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap("/Discussions/Mine?pageIndex=1")
            .To<DiscussionsController>(c =>
                c.Mine(1));
    }
}
