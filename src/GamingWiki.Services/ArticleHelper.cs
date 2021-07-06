using System.Linq;
using GamingWiki.Data;
using GamingWiki.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace GamingWiki.Services
{
    public class ArticleHelper : IArticleHelper
    {
        private readonly ApplicationDbContext dbContext;

        public ArticleHelper(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IdentityUser GetUser(string userId)
        {
            //var user = this.dbContext.ApplicationUsers
            //    .FirstOrDefault(u => u.Id == userId);

            return null;
        }
    }
}
