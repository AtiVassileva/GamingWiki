using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Web.Infrastructure;
using GamingWiki.Web.Models.Articles;
using static GamingWiki.Web.Common.WebConstants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IArticleService helper;

        public ArticlesController(IArticleService helper) 
            => this.helper = helper;

        [Authorize]
        public IActionResult All() 
            => this.View(new ArticleFullModel
            {
                Articles = this.helper.All(),
                Categories = this.helper.GetCategories()
            });

        [Authorize]
        public IActionResult Create() 
            => this.View(new ArticleFormModel
            {
                Categories = this.helper.GetCategories()
            });

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Create(ArticleFormModel model)
        {
            if (!this.helper.CategoryExists(model.CategoryId))
            {
                this.ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist.");
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

        [Authorize]
        public IActionResult Details(int articleId) 
            => this.View(this.helper.Details(articleId));

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int articleId)
        {
            var dbModel = this.helper.Details(articleId);

            var articleModel =  new ArticleServiceEditModel
                {
                    Id = dbModel.Id,
                    Heading = dbModel.Heading,
                    PictureUrl = dbModel.PictureUrl,
                    Content = dbModel.Content
                };

            return this.View(articleModel);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(ArticleServiceEditModel model, int articleId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            this.helper.Edit(articleId, model);

            return this.RedirectToAction(nameof(this.Details),
                new { articleId = $"{articleId}" });
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Delete(int articleId)
        {
            this.helper.Delete(articleId);
            return this.RedirectToAction(nameof(this.All));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Search(string searchCriteria) 
            => View(nameof(this.All), new ArticleFullModel
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