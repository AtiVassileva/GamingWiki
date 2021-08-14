using GamingWiki.Models;

namespace GamingWiki.Tests.Data
{
    public static class Games
    {
        private const int DefaultId = 7;
        public static Game TestGame => new () { Id = DefaultId };
    }
}
