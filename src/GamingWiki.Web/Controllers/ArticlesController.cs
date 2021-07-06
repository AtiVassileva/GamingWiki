using System;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Models.Enums;
using GamingWiki.Services;
using GamingWiki.Services.Contracts;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Articles;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IArticleHelper helper;
        private readonly IMapper mapper;

        public ArticlesController(ApplicationDbContext dbContext, IMapper mapper, IArticleHelper helper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.helper = new ArticleHelper(dbContext);
        }

        public IActionResult All()
        {
            var articleModels = this.dbContext.Articles
                .Select(a => new ArticleSimpleModel
                {
                    Id = a.Id,
                    Heading = a.Heading,
                    PictureUrl = a.PictureUrl,
                    Author = a.Author.UserName
                }).ToList();

            return this.View(articleModels);
        }

        public IActionResult Create()
        {
            var categories = Enum.GetValues<Category>()
                .Select(c => new string(c.ToString()));

            return this.View(categories);
        }

        [HttpPost]
        public IActionResult Create(ArticleFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Error", new ErrorViewModel
                {
                    RequestId = Guid.NewGuid().ToString()
                });
            }

            var articleDto = new ArticleDtoModel
            {
                Heading = model.Heading,
                Content = model.Content,
                Category = Enum.Parse<Category>(model.Category),
                AuthorId = this.User.FindFirstValue(ClaimTypes.NameIdentifier),
                PictureUrl = model.PictureUrl,
                PublishedOn = DateTime.UtcNow
        };

            var article = this.mapper.Map<Article>(articleDto);

            this.dbContext.Articles.Add(article);
            this.dbContext.SaveChanges();

            return this.Redirect($"/Articles/Details?articleId={article.Id}");
        }

        public IActionResult Details(int articleId)
        {
            var articleModel = this.dbContext.Articles
                .Where(a => a.Id == articleId)
                .Select(a => new ArticleListingModel
                {
                    Id = a.Id,
                    Heading = a.Heading,
                    Category = a.Category.ToString(),
                    Content = a.Content,
                    Author = a.Author.UserName,
                    PictureUrl = a.PictureUrl,
                    PublishedOn = a.PublishedOn.ToString("D")
                }).FirstOrDefault();


            return this.View(articleModel);
        }
    }
}
