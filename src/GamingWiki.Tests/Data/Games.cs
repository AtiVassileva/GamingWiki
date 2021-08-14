using GamingWiki.Models;
using static GamingWiki.Tests.Common.TestConstants;

namespace GamingWiki.Tests.Data
{
    public static class Games
    {
        public static Game TestGame => new () { Id = DefaultId };
    }
}
