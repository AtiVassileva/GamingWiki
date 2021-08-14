using System.Collections.Generic;
using AutoMapper;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Web.Infrastructure;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Articles;
using static GamingWiki.Web.Common.ExceptionMessages;
using static GamingWiki.Web.Common.AlertMessages;
using static GamingWiki.Web.Common.WebConstants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        private const int ArticlesPerPage = 2;

        private readonly IArticleService articleService;
        private readonly IMapper mapper;

        public ArticlesController(IArticleService helper, IMapper mapper)
        {
            this.articleService = helper;
            this.mapper = mapper;
        }
        
        public IActionResult All(int pageIndex = 1) 
            => this.View(new ArticleFullModel
            {
                Articles = PaginatedList<ArticleAllServiceModel>
                    .Create(this.articleService.All(),
                        pageIndex, ArticlesPerPage),
                Categories = this.articleService.GetCategories(),
                Tokens = new KeyValuePair<object, object>("All", null)
            });
        
        public IActionResult Create() 
            => this.View(new ArticleFormModel
            {
                Categories = this.articleService.GetCategories()
            });

        [HttpPost]
        public IActionResult Create(ArticleFormModel model)
        {
            if (!this.articleService.CategoryExists(model.CategoryId))
            {
                this.ModelState.AddModelError(nameof(model.CategoryId), NonExistingCategoryExceptionMessage);
            }

            if (!this.ModelState.IsValid)
            {
                model.Categories = this.articleService.GetCategories();
                return this.View(model);
            }

            var authorId = this.User.GetId();

            var articleId = this.articleService.Create(model.Heading, model.Content, model.CategoryId, model.PictureUrl,
                authorId);

            TempData[GlobalMessageKey] = SuccessfullyAddedArticleMessage;

            return this.RedirectToAction(nameof(this.Details),
                new { articleId });
        }
        
        public IActionResult Details(int articleId) 
            => this.articleService.ArticleExists(articleId) 
                ? this.View(this.articleService.Details(articleId))
                : this.View("Error", CreateError(NonExistingArticleExceptionMessage));

        public IActionResult Edit(int articleId)
        {
            if (!this.articleService.ArticleExists(articleId))
            {
                return this.View("Error",  CreateError(NonExistingArticleExceptionMessage));
            }

            var authorId = this.articleService.GetAuthorId(articleId);

            if (!this.User.IsAdmin() && this.User.GetId() != authorId)
            {
                return this.Unauthorized();
            }

            var detailsModel = this.articleService.Details(articleId);

            var articleModel = this.mapper
                .Map<ArticleServiceEditModel>(detailsModel);

            return this.View(articleModel);
        }

        [HttpPost]
        public IActionResult Edit(ArticleServiceEditModel model, int articleId)
        {
            if (!this.ModelState.IsValid)
            {
                var detailsModel = this.articleService.Details(articleId);

                model = this.mapper
                    .Map<ArticleServiceEditModel>(detailsModel);

                return this.View(model);
            }

            var edited = this.articleService.Edit(articleId, model);

            if (!edited)
            {
                return this.BadRequest();
            }

            TempData[GlobalMessageKey] = SuccessfullyEditedArticleMessage;

            return this.RedirectToAction(nameof(this.Details),
                new { articleId });
        }
        
        public IActionResult Delete(int articleId)
        {
            if (!this.articleService.ArticleExists(articleId))
            {
                return this.View("Error", CreateError(NonExistingArticleExceptionMessage));
            }

            var authorId = this.articleService.GetAuthorId(articleId);

            if (!this.User.IsAdmin() && this.User.GetId() != authorId)
            {
                return this.Unauthorized();
            }

            var deleted = this.articleService.Delete(articleId);

            if (!deleted)
            {
                return this.BadRequest();
            }

            TempData[GlobalMessageKey] = DeletedArticleMessage;

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Search(string parameter, int pageIndex = 1, string name = null) 
            => this.View(nameof(this.All), new ArticleFullModel
            {
                Articles = PaginatedList<ArticleAllServiceModel>
                    .Create(this.articleService.Search(parameter),
                        pageIndex, ArticlesPerPage),
                Categories = this.articleService.GetCategories(),
                Tokens = new KeyValuePair<object, object>("Search", parameter)
            });

        [HttpPost]
        public IActionResult Search(string searchCriteria, 
            int pageIndex = 1) 
            => this.View(nameof(this.All), new ArticleFullModel
            {
                Articles = PaginatedList<ArticleAllServiceModel>
                    .Create(this.articleService.Search(searchCriteria),
                        pageIndex, ArticlesPerPage),
                Categories = this.articleService.GetCategories(),
                Tokens = new KeyValuePair<object, object>("Search", searchCriteria)
            });

        public IActionResult Filter([FromQuery(Name = "parameter")]
            int categoryId, int pageIndex = 1) 
            => this.View(nameof(this.All), new ArticleFullModel
            {
                Articles = PaginatedList<ArticleAllServiceModel>
                    .Create(this.articleService.Filter(categoryId),
                        pageIndex, ArticlesPerPage),
                Categories = this.articleService.GetCategories(),
                Tokens = new KeyValuePair<object, object>("Filter", categoryId)
            });

        public IActionResult Mine(int pageIndex = 1) 
            => this.View(nameof(this.All), new ArticleFullModel
            {
                Articles = PaginatedList<ArticleAllServiceModel>
                    .Create(this.articleService.GetArticlesByUser
                            (this.User.GetId()),
                        pageIndex, ArticlesPerPage),
                Tokens = new KeyValuePair<object, object>("Mine", null)
            });
    }
}