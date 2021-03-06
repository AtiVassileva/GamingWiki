using System.Collections.Generic;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Services.Models.Categories;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Articles;
using static GamingWiki.Tests.Data.Articles;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Common.ExceptionMessages;
using MyTested.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace GamingWiki.Tests.Pipeline
{
    public class ArticlesPipelineTests
    {
        [Fact]
        public void AllWithoutPageShouldReturnCorrectViewWithCorrectModel()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Articles/All")
                .WithUser()
                .WithAntiForgeryToken())
            .To<ArticlesController>(c => c.All(With.No<int>()))
            .Which(controller => controller
                .WithData(FiveArticles))
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ArticleFullModel>(
                    m =>
                    {
                        m.Articles.ShouldBeOfType(typeof(PaginatedList<ArticleAllServiceModel>));
                        m.Categories.ShouldBeOfType(typeof(List<CategoryServiceModel>));
                        m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                    })
                .Passing(articleListing =>
                {
                    articleListing.Articles.Count.ShouldBe(2);
                    articleListing.Articles.PageIndex.ShouldBe(0);
                    articleListing.Articles.TotalPages.ShouldBe(3);
                }));

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(3, 1, 3)]
        [InlineData(2, 2, 3)]
        public void AllWithPageShouldReturnCorrectViewWithCorrectModel(int pageIndex, int expectedCountOnPage, int totalPages)
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Articles/All?pageIndex={pageIndex}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<ArticlesController>(c => c.All(pageIndex))
                .Which(controller => controller
                    .WithData(FiveArticles))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFullModel>(m =>
                    {
                        m.Articles.ShouldBeOfType(typeof(PaginatedList<ArticleAllServiceModel>));
                        m.Categories.ShouldBeOfType(typeof(List<CategoryServiceModel>));
                        m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                    })
                    .Passing(articleListing =>
                    {
                        articleListing.Articles.Count.ShouldBe(expectedCountOnPage);
                        articleListing.Articles.PageIndex.ShouldBe(pageIndex);
                        articleListing.Articles.TotalPages.ShouldBe(totalPages);
                    }));

        [Fact]
        public void PostCreateShouldBeMappedAndHaveInvalidModelStateAndReturnView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/Articles/Create")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<ArticlesController>(c => c.Create(new ArticleFormModel()))
                .Which()
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFormModel>());

        [Fact]
        public void GetCreateShouldBeMappedAndReturnCorrectView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation("/Articles/Create")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<ArticlesController>(c => c.Create())
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFormModel>());

        [Fact]
        public void DetailsShouldBeMappedAndReturnCorrectViewWithValidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Articles/Details?articleId={TestArticle.Id}")
                .WithUser()
                .WithAntiForgeryToken())
            .To<ArticlesController>(c => c.Details(TestArticle.Id))
            .Which(controller => controller
                .WithData(TestArticle))
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ArticleServiceDetailsModel>());

        [Fact]
        public void PostEditShouldBeMappedAndHaveInvalidModelStateAndReturnView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"/Articles/Edit?articleId={TestArticle.Id}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<ArticlesController>(c => c.Edit(new ArticleServiceEditModel(), TestArticle.Id))
                .Which(controller => controller
                    .WithData(TestArticle))
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleServiceEditModel>());

        [Fact]
        public void DetailsShouldBeMappedAndReturnErrorViewWithInvalidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Articles/Details?articleId={TestArticle.Id}")
                .WithUser()
                .WithAntiForgeryToken())
            .To<ArticlesController>(c => c.Details(TestArticle.Id))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ErrorViewModel>(m => 
                    m.Message == NonExistingArticleExceptionMessage));

        [Fact]
        public void GetEditShouldBeMappedAndReturnCorrectViewWithValidId()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Articles/Edit?articleId={TestArticle.Id}")
                    .WithUser(user => user.InRole(AdministratorRoleName))
                    .WithAntiForgeryToken())
                .To<ArticlesController>(c => c.Edit(TestArticle.Id))
                .Which(controller => controller
                    .WithData(TestArticle))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleServiceEditModel>());

        [Fact]
        public void GetEditShouldBeMappedAndReturnUnauthorizedForUnauthorizedUsers()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Articles/Edit?articleId={TestArticle.Id}")
                    .WithUser(TestUser.Identifier)
                    .WithAntiForgeryToken())
                .To<ArticlesController>(c => c.Edit(TestArticle.Id))
                .Which(controller => controller
                    .WithData(TestArticle))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void GetEditShouldBeMappedAndReturnErrorViewWithInvalidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Articles/Edit?articleId={TestArticle.Id}")
                .WithUser(TestUser.Identifier)
                .WithAntiForgeryToken())
            .To<ArticlesController>(c => c.Edit(TestArticle.Id))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ErrorViewModel>(m =>
                    m.Message == NonExistingArticleExceptionMessage));

        [Fact]
        public void DeleteShouldBeMappedAndRedirectUponSuccessfulAction()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Articles/Delete?articleId={TestArticle.Id}")
                    .WithUser(user => user.InRole(AdministratorRoleName))
                    .WithAntiForgeryToken())
                .To<ArticlesController>(c => c.Delete(TestArticle.Id))
                .Which(controller => controller
                    .WithData(TestArticle))
                .ShouldHave()
                .TempData(tempData => tempData
                        .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ArticlesController>(c => c
                        .All(With.Any<int>())));

        [Fact]
        public void DeleteShouldBeMappedAndReturnUnauthorizedForUnauthorizedUsers()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Articles/Delete?articleId={TestArticle.Id}")
                .WithUser(TestUser.Identifier)
                .WithAntiForgeryToken())
            .To<ArticlesController>(c => c.Delete(TestArticle.Id))
            .Which(controller => controller
                .WithData(TestArticle))
            .ShouldReturn()
            .Unauthorized();

        [Fact]
        public void DeleteShouldBeMappedAndReturnErrorViewWithInvalidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Articles/Delete?articleId={TestArticle.Id}")
                .WithUser(user => user.InRole(AdministratorRoleName))
                .WithAntiForgeryToken())
            .To<ArticlesController>(c =>
                c.Delete(TestArticle.Id))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ErrorViewModel>(m =>
                    m.Message == NonExistingArticleExceptionMessage));

        [Fact]
        public void GetSearchShouldBeMappedAndReturnCorrectViewWithPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Articles/Search?parameter=a&pageIndex=1")
                .WithUser()
                .WithAntiForgeryToken())
            .To<ArticlesController>(c =>
                c.Search("a", 1, With.No<string>()))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ArticleFullModel>(m =>
                {
                    m.Articles.ShouldBeOfType(typeof(PaginatedList<ArticleAllServiceModel>));
                    m.Categories.ShouldBeOfType(typeof(List<CategoryServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));

        [Fact]
        public void GetSearchShouldBeMappedAndReturnCorrectViewWithoutPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
            .WithLocation("/Articles/Search?parameter=a")
            .WithUser()
            .WithAntiForgeryToken())
            .To<ArticlesController>(c =>
        c.Search("a", With.No<int>(), With.No<string>()))
            .Which()
            .ShouldReturn()
            .View(view => view
            .WithModelOfType<ArticleFullModel>(m =>
            {
                m.Articles.ShouldBeOfType(typeof(PaginatedList<ArticleAllServiceModel>));
                m.Categories.ShouldBeOfType(typeof(List<CategoryServiceModel>));
                m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
            }));

        [Fact]
        public void GetSearchShouldBeMappedAndReturnCorrectViewWithName()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation("/Articles/Search?parameter=a&pageIndex=1&name=test")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<ArticlesController>(c =>
                    c.Search("a", 1, "test"))
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFullModel>(m =>
                    {
                        m.Articles.ShouldBeOfType(typeof(PaginatedList<ArticleAllServiceModel>));
                        m.Categories.ShouldBeOfType(typeof(List<CategoryServiceModel>));
                        m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                    }));

        [Fact]
        public void PostSearchShouldBeMappedAndReturnCorrectViewWithoutPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Articles/Search?searchCriteria=abc")
                .WithMethod(HttpMethod.Post)
                .WithUser()
                .WithAntiForgeryToken())
            .To<ArticlesController>(c =>
                c.Search("abc", With.No<int>()))
            .Which()
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post))
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ArticleFullModel>(m =>
                {
                    m.Articles.ShouldBeOfType(typeof(PaginatedList<ArticleAllServiceModel>));
                    m.Categories.ShouldBeOfType(typeof(List<CategoryServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));

        [Fact]
        public void PostSearchShouldBeMappedAndReturnCorrectViewWithPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Articles/Search?searchCriteria=abc&pageIndex=1")
                .WithMethod(HttpMethod.Post)
                .WithUser()
                .WithAntiForgeryToken())
            .To<ArticlesController>(c =>
                c.Search("abc", 1))
            .Which()
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post))
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ArticleFullModel>(m =>
                {
                    m.Articles.ShouldBeOfType(typeof(PaginatedList<ArticleAllServiceModel>));
                    m.Categories.ShouldBeOfType(typeof(List<CategoryServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));

        [Fact]
        public void FilterShouldBeMappedAndReturnCorrectViewWithoutPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Articles/Filter?parameter=1")
                .WithUser()
                .WithAntiForgeryToken())
            .To<ArticlesController>(c =>
                c.Filter(1, With.No<int>()))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ArticleFullModel>(m =>
                {
                    m.Articles.ShouldBeOfType(typeof(PaginatedList<ArticleAllServiceModel>));
                    m.Categories.ShouldBeOfType(typeof(List<CategoryServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));

        [Fact]
        public void FilterShouldBeMappedAndReturnCorrectViewWithPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Articles/Filter?parameter=1&pageIndex=1")
                .WithUser()
                .WithAntiForgeryToken())
            .To<ArticlesController>(c =>
                c.Filter(1, 1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ArticleFullModel>(m =>
                {
                    m.Articles.ShouldBeOfType(typeof(PaginatedList<ArticleAllServiceModel>));
                    m.Categories.ShouldBeOfType(typeof(List<CategoryServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));

        [Fact]
        public void MineShouldBeMappedAndReturnCorrectViewWithPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Articles/Mine?pageIndex=1")
                .WithUser()
                .WithAntiForgeryToken())
            .To<ArticlesController>(c =>
                c.Mine(1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ArticleFullModel>(m =>
                {
                    m.Articles.ShouldBeOfType(typeof(PaginatedList<ArticleAllServiceModel>));
                    m.Categories.ShouldBeNull();
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));

        [Fact]
        public void MineShouldBeMappedAndReturnCorrectViewWithoutPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Articles/Mine")
                .WithUser()
                .WithAntiForgeryToken())
            .To<ArticlesController>(c =>
                c.Mine(With.No<int>()))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ArticleFullModel>(m =>
                {
                    m.Articles.ShouldBeOfType(typeof(PaginatedList<ArticleAllServiceModel>));
                    m.Categories.ShouldBeNull();
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));

    }
}
