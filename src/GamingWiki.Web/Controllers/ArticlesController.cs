using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Web.Infrastructure;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Articles;
using static GamingWiki.Web.Common.ExceptionMessages;
using static GamingWiki.Web.Common.WebConstants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        private const int ArticlesPerPage = 2;

        private readonly IArticleService helper;
        private readonly IMapper mapper;

        public ArticlesController(IArticleService helper, IMapper mapper)
        {
            this.helper = helper;
            this.mapper = mapper;
        }
        
        public IActionResult All(int pageIndex = 1) 
            => this.View(new ArticleFullModel
            {
                Articles = PaginatedList<ArticleAllServiceModel>
                    .Create(this.helper.All(),
                        pageIndex, ArticlesPerPage),
                Categories = this.helper.GetCategories(),
                Tokens = new KeyValuePair<object, object>("All", null)
            });
        
        public IActionResult Create() 
            => this.View(new ArticleFormModel
            {
                Categories = this.helper.GetCategories()
            });

        [HttpPost]
        public IActionResult Create(ArticleFormModel model)
        {
            if (!this.helper.CategoryExists(model.CategoryId))
            {
                this.ModelState.AddModelError(nameof(model.CategoryId), NonExistingCategoryExceptionMessage);
            }

            if (!this.ModelState.IsValid)
            {
                model.Categories = this.helper.GetCategories();
                return this.View(model);
            }

            var authorId = this.User.GetId();

            var articleId = this.helper.Create(model.Heading, model.Content, model.CategoryId, model.PictureUrl,
                authorId);

            return this.RedirectToAction(nameof(this.Details),
                new { articleId = $"{articleId}" });
        }
        
        public IActionResult Details(int articleId)
            => this.helper.ArticleExists(articleId) ? 
                this.View(this.helper.Details(articleId)) 
                : this.View("Error", CreateError(NonExistingArticleExceptionMessage));
        
        public IActionResult Edit(int articleId)
        {
            if (!this.helper.ArticleExists(articleId))
            {
                return this.View("Error",  CreateError(NonExistingArticleExceptionMessage));
            }

            var authorId = this.helper.GetAuthorId(articleId);

            if (!this.User.IsAdmin() && this.User.GetId() != authorId)
            {
                return this.Unauthorized();
            }

            var detailsModel = this.helper.Details(articleId);

            var articleModel = this.mapper
                .Map<ArticleServiceEditModel>(detailsModel);

            return this.View(articleModel);
        }

        [HttpPost]
        public IActionResult Edit(ArticleServiceEditModel model, int articleId)
        {
            if (!this.ModelState.IsValid)
            {
                var detailsModel = this.helper.Details(articleId);

                model = this.mapper
                    .Map<ArticleServiceEditModel>(detailsModel);

                return this.View(model);
            }

            this.helper.Edit(articleId, model);

            return this.RedirectToAction(nameof(this.Details),
                new { articleId = $"{articleId}" });
        }
        
        public IActionResult Delete(int articleId)
        {
            if (!this.helper.ArticleExists(articleId))
            {
                return this.View("Error", CreateError(NonExistingArticleExceptionMessage));
            }

            var authorId = this.helper.GetAuthorId(articleId);

            if (!this.User.IsAdmin() && this.User.GetId() != authorId)
            {
                return this.Unauthorized();
            }

            this.helper.Delete(articleId);
            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Search(string parameter, int pageIndex = 1, string name = null)
        {
            return this.View(nameof(this.All), new ArticleFullModel
            {
                Articles = PaginatedList<ArticleAllServiceModel>
                    .Create(this.helper.Search(parameter),
                        pageIndex, ArticlesPerPage),
                Categories = this.helper.GetCategories(),
                Tokens = new KeyValuePair<object, object>("Search", parameter)
            });
        }

        [HttpPost]
        public IActionResult Search(string searchCriteria, int pageIndex = 1)
        {
            return this.View(nameof(this.All), new ArticleFullModel
            {
                Articles = PaginatedList<ArticleAllServiceModel>
                    .Create(this.helper.Search(searchCriteria),
                        pageIndex, ArticlesPerPage),
                Categories = this.helper.GetCategories(),
                Tokens = new KeyValuePair<object, object>("Search", searchCriteria)
            });
        }

        public IActionResult Filter([FromQuery(Name = "parameter")]
            int categoryId, int pageIndex = 1) 
            => this.View(nameof(this.All), new ArticleFullModel
            {
                Articles = PaginatedList<ArticleAllServiceModel>
                    .Create(this.helper.Filter(categoryId),
                        pageIndex, ArticlesPerPage),
                Categories = this.helper.GetCategories(),
                Tokens = new KeyValuePair<object, object>("Filter", categoryId)
            });

        public IActionResult Mine(int pageIndex = 1) 
            => this.View(nameof(this.All), new ArticleFullModel()
            {
                Articles = PaginatedList<ArticleAllServiceModel>
                    .Create(this.helper.GetArticlesByUser
                            (this.User.GetId()),
                        pageIndex, ArticlesPerPage),
                Tokens = new KeyValuePair<object, object>("Mine", null)
            });
    }
}