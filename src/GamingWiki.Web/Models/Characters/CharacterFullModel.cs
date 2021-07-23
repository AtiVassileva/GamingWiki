using System.Collections.Generic;
using GamingWiki.Web.Models.Classes;

namespace GamingWiki.Web.Models.Characters
{
    public class CharacterFullModel
    {
        public IEnumerable<CharacterSimpleModel> Characters { get; set; }

        public IEnumerable<ClassViewModel> Classes { get; set; }
    }
}
