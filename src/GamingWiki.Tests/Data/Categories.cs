using GamingWiki.Models;
using static GamingWiki.Tests.Common.TestConstants;

namespace GamingWiki.Tests.Data
{
    public static class Categories
    {
        public static Category TestCategory => new()
        {
            Id = DefaultId
        };
    }
}
