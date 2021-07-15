using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Articles;
using GamingWiki.Web.Models.Categories;
using GamingWiki.Web.Models.Comments;
using GamingWiki.Web.Models.Replies;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public ArticlesController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public IActionResult All()
        {
            var articleModels = this.dbContext.Articles
                .Select(a => new ArticleSimpleModel
                {
                    Id = a.Id,
                    Heading = a.Heading,
                    PictureUrl = a.PictureUrl,
                    PublishedOn = a.PublishedOn.ToString("D")
                })
                .OrderByDescending(c => c.Id)
                .ToList();

            return this.View(articleModels);
        }

        public IActionResult Create() 
            => this.View(new ArticleFormModel
            {
                Categories = this.GetCategories()
            });

        [HttpPost]
        public IActionResult Create(ArticleFormModel model)
        {
            if (!this.dbContext.Categories.Any(c => c.Id == model.CategoryId))
            {
                this.ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist.");
            }

            if (!this.ModelState.IsValid)
            {
                model.Categories = this.GetCategories();

                return this.View(model);
            }

            var articleDto = new ArticleDtoModel
            {
                Heading = model.Heading,
                Content = model.Content,
                CategoryId = model.CategoryId,
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
                    PublishedOn = a.PublishedOn.ToString("f"),
                    Comments = a.Comments.Select(c =>
                        new CommentListingModel
                        {
                            Id = c.Id,
                            Content = c.Content,
                            Commenter = c.Commenter.UserName,
                            AddedOn = c.AddedOn.ToString("dd/MM/yyyy"),
                            Replies = c.Replies.Select(r => 
                                new ReplyListingModel
                            {
                                    Id = r.Id,
                                    Content = r.Content,
                                    Replier = r.Replier.UserName
                            }).ToList()
                        }).ToList()
                }).FirstOrDefault();


            return this.View(articleModel);
        }

        public IActionResult Edit(int articleId)
        {
            var articleModel = this.dbContext.Articles
                .Where(a => a.Id == articleId)
                .Select(a => new ArticleEditModel
                {
                    Id = a.Id,
                    Heading = a.Heading,
                    PictureUrl = a.PictureUrl,
                    Content = a.Content

                }).FirstOrDefault();

            return this.View(articleModel);
        }

        [HttpPost]
        public IActionResult Edit(ArticleEditModel model, int articleId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var article = this.dbContext.Articles
                .First(a => a.Id == articleId);

            article.Heading = model.Heading;
            article.PictureUrl = model.PictureUrl;
            article.Content = model.Content;

            this.dbContext.SaveChanges();

            return this.Redirect($"/Articles/Details?articleId={article.Id}");
        }

        public IActionResult Delete(int articleId)
        {
            var article = this.dbContext.Articles
                .First(a => a.Id == articleId);

            this.dbContext.Articles.Remove(article);
            this.dbContext.SaveChanges();

            return this.Redirect("/Articles/All");
        }

        [HttpPost]
        public IActionResult Search(string searchCriteria)
        {
            var articleModels = this.dbContext.Articles
                .Where(a => a.Heading.ToLower().Contains(searchCriteria.ToLower().Trim()))
                .Select(a => new ArticleSimpleModel
                {
                    Id = a.Id,
                    Heading = a.Heading,
                    PictureUrl = a.PictureUrl,
                    PublishedOn = a.PublishedOn.ToString("D")
                }).ToList();

            return View("All", articleModels);
        }

        private IEnumerable<CategoryViewModel> GetCategories()
            => this.dbContext.Categories
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();
    }
}