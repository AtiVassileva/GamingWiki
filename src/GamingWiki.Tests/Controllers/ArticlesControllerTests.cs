using System;
using System.Linq;
using GamingWiki.Models;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Articles;
using MyTested.AspNetCore.Mvc;
using Xunit;
using static GamingWiki.Tests.Data.Articles;
using static GamingWiki.Tests.Data.Categories;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Tests.Common.TestConstants;
using TestUser = MyTested.AspNetCore.Mvc.TestUser;

namespace GamingWiki.Tests.Controllers
{
    public class ArticlesControllerTests
    {
        [Fact]
        public void ArticlesControllerShouldBeForAuthorizedUsersOnly()
        => MyController<ArticlesController>
            .ShouldHave()
            .Attributes(attributes => attributes
                .RestrictingForAuthorizedRequests());

        [Fact]
        public void AllShouldReturnCorrectViewWithModel()
            => MyController<ArticlesController>
                .Instance(controller => controller
                    .WithData(FiveArticles))
                .Calling(c => c.All(DefaultPageIndex))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFullModel>());

        [Fact]
        public void GetCreateShouldReturnCorrectView()
            => MyController<ArticlesController>
                .Instance()
                .Calling(a => a.Create())
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFormModel>());

        [Fact]
        public void PostCreateShouldBeForAuthorizedUsersAndReturnRedirectWithValidModel()
            => MyController<ArticlesController>
                .Instance(controller => controller
                    .WithData(TestCategory)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Create(TestArticleFormModel))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                    .WithSet<Article>(articles =>
                        articles.Any(a =>
                        a.Heading == TestArticleFormModel.Heading
                        && a.Content == TestArticleFormModel.Content)
                    ))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ArticlesController>
                        (c => c.Details(With.Any<int>())));

        [Fact]
        public void PostCreateShouldReturnViewWithInvalidModelState()
            => MyController<ArticlesController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Create(TestArticleFormModel))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFormModel>());

        [Fact]
        public void DetailsShouldReturnErrorViewWithInvalidArticleId()
            => MyController<ArticlesController>
                .Instance()
                .Calling(c => c.Details(TestArticle.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>());

        [Fact]
        public void DetailsShouldReturnCorrectViewWithValidArticleId()
            => MyController<ArticlesController>
                .Instance(controller => controller
                    .WithData(TestArticle))
                .Calling(c => c.Details(TestArticle.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleServiceDetailsModel>());

        [Fact]
        public void GetEditShouldReturnErrorViewWithInvalidId()
            => MyController<ArticlesController>
                .Instance()
                .Calling(c => c.Edit(TestArticle.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>());

        [Fact]
        public void GetEditShouldReturnCorrectViewWithValidId()
            => MyController<ArticlesController>
                .Instance(controller => controller
                    .WithUser(user => user
                        .InRole(AdministratorRoleName))
                    .WithData(TestArticle))
                .Calling(c => c.Edit(TestArticle.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleServiceEditModel>());

        [Fact]
        public void GetEditShouldReturnUnauthorizedForUnauthorizedUsers()
            => MyController<ArticlesController>
                .Instance(controller => controller
                    .WithData(TestArticle))
                .Calling(c => c.Edit(TestArticle.Id))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void PostEditShouldReturnRedirectWithValidModel()
            => MyController<ArticlesController>
                .Instance(controller => controller
                    .WithData(TestArticle)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Edit(TestValidArticleEditModel, TestArticle.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                    .WithSet<Article>(articles => articles
                        .Any(a =>
                            a.Heading == TestArticleFormModel.Heading
                            && a.Content == TestArticleFormModel.Content)
                    )).TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ArticlesController>(c =>
                        c.Details(With.Any<int>())));

        [Fact]
        public void PostEditShouldReturnViewWithInvalidModelState()
            => MyController<ArticlesController>
                .Instance(controller => controller
                    .WithData(TestArticle)
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Edit(TestInvalidArticleEditModel, TestArticle.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleServiceEditModel>());

        [Fact]
        public void PostEditShouldReturnBadRequestUponUnsuccessfulEdition()
            => MyController<ArticlesController>
                .Instance(controller => controller
                    .WithUser(TestUser.Identifier))
                .Calling(c => c.Edit(TestValidArticleEditModel, TestArticle.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .AndAlso()
                .ShouldReturn()
                .BadRequest();

        [Fact]
        public void DeleteShouldReturnErrorViewWithInvalidArticleId()
            => MyController<ArticlesController>
                .Instance()
                .Calling(c => c.Delete(TestArticle.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ErrorViewModel>());

        [Fact]
        public void DeleteShouldReturnUnauthorizedForUnauthorizedUsers()
            => MyController<ArticlesController>
                .Instance(controller => controller
                    .WithData(TestArticle))
                .Calling(c => c.Delete(TestArticle.Id))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void DeleteShouldReturnRedirectWithValidId()
            => MyController<ArticlesController>
                .Instance(controller => controller
                    .WithData(TestArticle)
                    .WithUser(user => user
                        .InRole(AdministratorRoleName)))
                .Calling(c => c.Delete(TestArticle.Id))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Article>(articles => articles
                        .All(a =>
                            a.Heading != TestArticleFormModel.Heading
                            && a.Content != TestArticleFormModel.Content)
                    )).TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ArticlesController>(c =>
                        c.All(With.Any<int>())));

        [Fact]
        public void GetSearchShouldReturnCorrectViewWithModel()
            => MyController<ArticlesController>
                .Instance(controller => controller
                        .WithData(FiveArticles))
                .Calling(c => c.Search(Guid.NewGuid().ToString(), 
                    DefaultPageIndex, null))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFullModel>());

        [Fact]
        public void PostSearchShouldReturnCorrectViewWithModel()
            => MyController<ArticlesController>
                .Instance(controller => controller
                        .WithData(FiveArticles))
                .Calling(c => c.Search(Guid.NewGuid().ToString(), 
                    DefaultPageIndex))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFullModel>());

        [Fact]
        public void FilterShouldReturnCorrectViewWithModel()
            => MyController<ArticlesController>
                .Instance(controller => controller
                    .WithData(FiveArticles))
                .Calling(c => c.Filter(new Random().Next(), DefaultPageIndex))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFullModel>());

        [Fact]
        public void MineShouldReturnCorrectViewWithModel()
            => MyController<ArticlesController>
                .Instance(controller => controller
                        .WithData(FiveArticles))
                .Calling(c => c.Mine(DefaultPageIndex))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFullModel>());
    }
}
