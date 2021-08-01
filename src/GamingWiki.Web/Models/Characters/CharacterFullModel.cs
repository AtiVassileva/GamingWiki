using System.Collections.Generic;
using GamingWiki.Services.Models.Characters;
using GamingWiki.Services.Models.Classes;

namespace GamingWiki.Web.Models.Characters
{
    public class CharacterFullModel
    {
        public PaginatedList<CharacterAllServiceModel> Characters { get; set; }

        public IEnumerable<ClassSimpleServiceModel> Classes { get; set; }

        public KeyValuePair<object, object> Tokens { get; set; } = new();
    }
}
