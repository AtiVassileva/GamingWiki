using System.Collections.Generic;

namespace GamingWiki.Web.Models.Characters
{
    public class CharacterCollectionsModel
    {
        public IEnumerable<string> GameNames { get; set; }

        public IEnumerable<string> CharacterClasses { get; set; }
    }
}
