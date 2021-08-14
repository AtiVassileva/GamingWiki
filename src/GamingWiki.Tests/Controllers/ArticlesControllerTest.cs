using System;
using System.Linq;
using GamingWiki.Models;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models.Articles;
using MyTested.AspNetCore.Mvc;
using Xunit;
using static GamingWiki.Tests.Data.Articles;
using static GamingWiki.Tests.Data.Categories;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using static GamingWiki.Web.Common.WebConstants;
using TestUser = MyTested.AspNetCore.Mvc.TestUser;

namespace GamingWiki.Tests.Controllers
{
    public class ArticlesControllerTest
    {
        private const int DefaultPageIndex = 1;

        [Fact]
        public void ArticlesControllerShouldBeForAuthorizedUsersOnly()
        => MyController<ArticlesController>
            .ShouldHave()
            .Attributes(attributes => attributes
                .RestrictingForAuthorizedRequests());

        [Fact]
        public void AllShouldReturnCorrectViewWithModel()
            => MyController<ArticlesController>
                .Instance(instance =>
                    instance.WithData(FiveArticles))
                .Calling(a => a.All(DefaultPageIndex))
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
                    .WithUser(TestUser.Username, TestUser.Identifier))
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
                    .WithUser(TestUser.Username, TestUser.Identifier))
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
                .Calling(a => a.Details(new Random().Next()))
                .ShouldReturn()
                .View("Error");

        [Fact]
        public void DetailsShouldReturnCorrectViewWithValidArticleId()
            => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithData(TestArticle))
                .Calling(a => a.Details(With.Value(TestArticle.Id)))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleServiceDetailsModel>());

        [Fact]
        public void GetEditShouldReturnErrorViewWithInvalidId()
            => MyController<ArticlesController>
                .Instance()
                .Calling(a => a.Edit(new Random().Next()))
                .ShouldReturn()
                .View("Error");

        [Fact]
        public void GetEditShouldReturnCorrectViewWithValidId()
            => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithUser(user => user
                        .InRole(AdministratorRoleName))
                    .WithData(TestArticle))
                .Calling(a => a.Edit(TestArticle.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleServiceEditModel>());

        [Fact]
        public void GetEditShouldReturnUnauthorizedForUnauthorizedUsers()
            => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithData(TestArticle))
                .Calling(a => a.Edit(TestArticle.Id))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void PostEditShouldReturnRedirectWithValidModel()
            => MyController<ArticlesController>
                .Instance(controller => controller
                    .WithData(TestArticle)
                    .WithUser(TestUser.Username, TestUser.Identifier))
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
                    .WithUser(TestUser.Username, TestUser.Identifier))
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
                    .WithUser(TestUser.Username, TestUser.Identifier))
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
                .View("Error");

        [Fact]
        public void DeleteShouldReturnUnauthorizedForUnauthorizedUsers()
            => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithData(TestArticle))
                .Calling(a => a.Delete(TestArticle.Id))
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
                    .WithSet<Article>(articles => !articles
                        .Any(a =>
                            a.Heading == TestArticleFormModel.Heading
                            && a.Content == TestArticleFormModel.Content)
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
                .Instance(instance =>
                    instance.WithData(FiveArticles))
                .Calling(a => a.Search(Guid.NewGuid().ToString(), DefaultPageIndex, null))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFullModel>());

        [Fact]
        public void PostSearchShouldReturnCorrectViewWithModel()
            => MyController<ArticlesController>
                .Instance(instance =>
                    instance.WithData(FiveArticles))
                .Calling(a => a.Search(Guid.NewGuid().ToString(), DefaultPageIndex))
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
                .Instance(instance =>
                    instance.WithData(FiveArticles))
                .Calling(a => a.Filter(new Random().Next(), DefaultPageIndex))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFullModel>());

        [Fact]
        public void MineShouldReturnCorrectViewWithModel()
            => MyController<ArticlesController>
                .Instance(instance =>
                    instance.WithData(FiveArticles))
                .Calling(a => a.Mine(DefaultPageIndex))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFullModel>());
    }
}
