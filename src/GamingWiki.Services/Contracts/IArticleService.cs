using System.Collections.Generic;
using System.Linq;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Services.Models.Categories;

namespace GamingWiki.Services.Contracts
{
    public interface IArticleService
    {
        IQueryable<ArticleAllServiceModel> All();

        int Create(string heading, string content, int categoryId,
            string pictureUrl, string authorId);

        ArticleServiceDetailsModel Details(int articleId);

        void Edit(int articleId, ArticleServiceEditModel model);

        void Delete(int articleId);

        bool CategoryExists(int categoryId);

        string GetAuthorId(int articleId);

        bool ArticleExists(int articleId);

        IQueryable<ArticleAllServiceModel> Search(string searchCriteria);

        IQueryable<ArticleAllServiceModel> Filter(int categoryId);

        IEnumerable<CategoryServiceModel> GetCategories();

        IEnumerable<ArticleServiceHomeModel> GetLatest();

        IQueryable<ArticleAllServiceModel> GetArticlesByUser(string userId);
    }
}
