using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Services.Models.Categories;
using Microsoft.EntityFrameworkCore;
using static GamingWiki.Services.Common.ServiceConstants;

namespace GamingWiki.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ICommentService commentService;
        private readonly IMapper mapper;
        private readonly IConfigurationProvider configuration;

        public ArticleService(ApplicationDbContext dbContext, ICommentService commentService, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.commentService = commentService;
            this.mapper = mapper;
            this.configuration = mapper.ConfigurationProvider;
        }

        public IQueryable<ArticleAllServiceModel> All()
            => this.GetArticles(this.dbContext.Articles)
                .OrderByDescending(a => a.Id);

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
        {
            var article = this.FindArticle(articleId);

            var articleDetails = this.mapper
                .Map<ArticleServiceDetailsModel>(article);
            
            articleDetails.Comments = this.commentService
                .AllByArticle(articleDetails.Id);

            return articleDetails;
        }

        public bool Edit(int articleId, ArticleServiceEditModel model)
        {
            if (!this.ArticleExists(articleId))
            {
                return false;
            }

            var article = this.FindArticle(articleId);

            article.Heading = model.Heading;
            article.PictureUrl = model.PictureUrl;
            article.Content = model.Content;

            this.dbContext.SaveChanges();

            return true;
        }

        public bool Delete(int articleId)
        {
            if (!this.ArticleExists(articleId))
            {
                return false;
            }

            var article = this.FindArticle(articleId);

            this.dbContext.Articles.Remove(article);
            this.dbContext.SaveChanges();

            return true;
        }

        public bool CategoryExists(int categoryId)
            => this.dbContext.Categories
                .Any(c => c.Id == categoryId);

        public bool ArticleExists(int articleId)
            => this.dbContext.Articles.Any(a => a.Id == articleId);

        public string GetAuthorId(int articleId)
            => this.dbContext.Articles
                .First(a => a.Id == articleId).AuthorId;

        public IQueryable<ArticleAllServiceModel> Search(string searchCriteria)
            => this.GetArticles(this.dbContext.Articles
                .Where(a => a.Heading.ToLower().Contains(searchCriteria.ToLower().Trim())))
                .OrderByDescending(a => a.Id);

        public IQueryable<ArticleAllServiceModel> Filter(int categoryId)
            => GetArticles(this.dbContext.Articles
                .Where(a => a.CategoryId == categoryId))
                .OrderByDescending(a => a.Id);

        public IEnumerable<CategoryServiceModel> GetCategories()
        => this.dbContext.Categories
            .ProjectTo<CategoryServiceModel>(this.configuration)
            .OrderByDescending(c => c.Id)
            .ToList();

        public IEnumerable<ArticleServiceHomeModel> GetLatest()
        => this.dbContext.Articles
            .OrderByDescending(a => a.Id)
            .ProjectTo<ArticleServiceHomeModel>(this.configuration)
            .Take(HomePageEntityCount)
            .ToList();

        public IQueryable<ArticleAllServiceModel> GetArticlesByUser(string userId)
            => this.GetArticles(this.dbContext.Articles
                .Where(a => a.AuthorId == userId))
                .OrderByDescending(a => a.Id);

        private Article FindArticle(int articleId)
            => this.dbContext.Articles
                .Include(a => a.Author)
                .First(a => a.Id == articleId);

        private IQueryable<ArticleAllServiceModel> GetArticles(IQueryable articlesQuery)
        => articlesQuery
            .ProjectTo<ArticleAllServiceModel>(this.configuration);

    }
}
