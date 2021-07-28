using System;
using System.Collections.Generic;
using System.Linq;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Services.Models.Categories;

namespace GamingWiki.Services
{
    public class ArticleService : IArticleService
    {
        private const string AllDateFormat = "D";
        private const string DetailsDateFormat = "f";

        private const int HomePageEntityCount = 3;

        private readonly ApplicationDbContext dbContext;
        private readonly ICommentService commentService;

        public ArticleService(ApplicationDbContext dbContext, ICommentService commentService)
        {
            this.dbContext = dbContext;
            this.commentService = commentService;
        }

        public IEnumerable<ArticleAllServiceModel> All()
            => GetArticles(this.dbContext.Articles);

        public int Create(string heading, string content, int categoryId,
            string pictureUrl, string authorId)
        {
            var article = new Article
            {
                Heading = heading,
                Content = content,
                CategoryId = categoryId,
                AuthorId = authorId,
                PictureUrl = pictureUrl,
                PublishedOn = DateTime.UtcNow
            };

            this.dbContext.Articles.Add(article);
            this.dbContext.SaveChanges();

            return article.Id;
        }

        public ArticleServiceDetailsModel Details(int articleId)
            => this.dbContext.Articles
                .Where(a => a.Id == articleId)
                .Select(a => new ArticleServiceDetailsModel
                {
                    Id = a.Id,
                    Heading = a.Heading,
                    Category = a.Category.ToString(),
                    Content = a.Content,
                    Author = a.Author.UserName,
                    AuthorId = a.AuthorId,
                    PictureUrl = a.PictureUrl,
                    PublishedOn = a.PublishedOn.ToString(DetailsDateFormat),
                    Comments = this.commentService.AllByArticle(a.Id),
                }).FirstOrDefault();

        public void Edit(int articleId, ArticleServiceEditModel model)
        {
            var article = this.dbContext.Articles
                .FirstOrDefault(a => a.Id == articleId);

            if (article == null)
            {
                return;
            }

            article.Heading = model.Heading;
            article.PictureUrl = model.PictureUrl;
            article.Content = model.Content;

            this.dbContext.SaveChanges();
        }

        public void Delete(int articleId)
        {
            var article = this.dbContext.Articles
                .FirstOrDefault(a => a.Id == articleId);

            if (article == null)
            {
                return;
            }

            this.dbContext.Articles.Remove(article);
            this.dbContext.SaveChanges();
        }

        public bool CategoryExists(int categoryId)
            => this.dbContext.Categories
                .Any(c => c.Id == categoryId);

        public string GetAuthorId(int articleId)
            => this.dbContext.Articles
                .First(a => a.Id == articleId).AuthorId;

        public bool ArticleExists(int articleId)
            => this.dbContext.Articles.Any(a => a.Id == articleId);

        public IEnumerable<ArticleAllServiceModel> Search(string searchCriteria)
            => GetArticles(this.dbContext.Articles
                .Where(a => a.Heading.ToLower().Contains(searchCriteria.ToLower().Trim())));

        public IEnumerable<ArticleAllServiceModel> Filter(int categoryId)
            => GetArticles(this.dbContext.Articles
                .Where(a => a.CategoryId == categoryId));

        public IEnumerable<CategoryServiceModel> GetCategories()
        => this.dbContext.Categories
            .Select(c => new CategoryServiceModel
            {
                Id = c.Id,
                Name = c.Name
            })
            .OrderByDescending(c => c.Id)
            .ToList();

        public IEnumerable<ArticleServiceHomeModel> GetLatest()
        => this.dbContext.Articles
            .OrderByDescending(a => a.Id)
            .Select(a => new ArticleServiceHomeModel
            {
                Id = a.Id,
                Heading = a.Heading,
                PictureUrl = a.PictureUrl,
                ShortContent = a.Content.Substring(0, 200)
            })
            .Take(HomePageEntityCount)
            .ToList();


        private static IEnumerable<ArticleAllServiceModel> GetArticles(IQueryable<Article> articlesQuery)
        => articlesQuery.Select(a
                => new ArticleAllServiceModel
                {
                    Id = a.Id,
                    Heading = a.Heading,
                    PictureUrl = a.PictureUrl,
                    PublishedOn = a.PublishedOn.ToString(AllDateFormat)
                })
            .OrderByDescending(c => c.Id)
            .ToList();

    }
}
