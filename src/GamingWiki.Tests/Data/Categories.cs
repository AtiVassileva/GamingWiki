using GamingWiki.Models;

namespace GamingWiki.Tests.Data
{
    public static class Categories
    {
        private const int DefaultCategoryId = 3;
        public static Category TestCategory => new()
        {
            Id = DefaultCategoryId
        };
    }
}
