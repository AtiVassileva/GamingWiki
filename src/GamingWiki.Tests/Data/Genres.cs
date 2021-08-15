using GamingWiki.Models;
using static GamingWiki.Tests.Common.TestConstants;

namespace GamingWiki.Tests.Data
{
    public static class Genres
    {
        public static Genre TestGenre => new() { Id = DefaultId };
    }
}
