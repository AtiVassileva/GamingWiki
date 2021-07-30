using AutoMapper;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Web.Infrastructure;
using GamingWiki.Web.Models.Articles;
using static GamingWiki.Web.Common.ExceptionMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        private readonly IArticleService helper;
        private readonly IMapper mapper;

        public ArticlesController(IArticleService helper, IMapper mapper)
        {
            this.helper = helper;
            this.mapper = mapper;
        }
        
        public IActionResult All() 
            => this.View(new ArticleFullModel
            {
                Articles = this.helper.All(),
                Categories = this.helper.GetCategories()
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
                : this.View("Error", NonExistingArticleExceptionMessage);
        
        public IActionResult Edit(int articleId)
        {
            if (!this.helper.ArticleExists(articleId))
            {
                return this.View("Error",  NonExistingArticleExceptionMessage);
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
                return this.View("Error", NonExistingArticleExceptionMessage);
            }

            var authorId = this.helper.GetAuthorId(articleId);

            if (!this.User.IsAdmin() && this.User.GetId() != authorId)
            {
                return this.Unauthorized();
            }

            this.helper.Delete(articleId);
            return this.RedirectToAction(nameof(this.All));
        }

        [HttpPost]
        public IActionResult Search(string searchCriteria) 
            => this.View(nameof(this.All), new ArticleFullModel
            {
                Articles = this.helper.Search(searchCriteria),
                Categories = this.helper.GetCategories()
            });

        public IActionResult Filter(int categoryId) 
            => this.View(nameof(this.All), new ArticleFullModel
            {
                Articles = this.helper.Filter(categoryId),
                Categories = this.helper.GetCategories()
            });
    }
}