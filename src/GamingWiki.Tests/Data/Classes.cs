using GamingWiki.Models;

namespace GamingWiki.Tests.Data
{
    public static class Classes
    {
        private const int DefaultId = 5;
        public static Class TestCharacterClass => new (){Id = DefaultId};
    }
}
