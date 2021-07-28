using System.Collections.Generic;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Services.Models.Categories;

namespace GamingWiki.Services.Contracts
{
    public interface IArticleService
    {
        IEnumerable<ArticleAllServiceModel> All();

        int Create(string heading, string content, int categoryId,
            string pictureUrl, string authorId);

        ArticleServiceDetailsModel Details(int articleId);

        void Edit(int articleId, ArticleServiceEditModel model);

        void Delete(int articleId);

        bool CategoryExists(int categoryId);

        string GetAuthorId(int articleId);

        bool ArticleExists(int articleId);

        IEnumerable<ArticleAllServiceModel> Search(string searchCriteria);

        IEnumerable<ArticleAllServiceModel> Filter(int categoryId);

        IEnumerable<CategoryServiceModel> GetCategories();

        IEnumerable<ArticleServiceHomeModel> GetLatest();
    }
}
