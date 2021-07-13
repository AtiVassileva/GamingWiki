using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static GamingWiki.Models.Common.DataConstants;

namespace GamingWiki.Web.Models.Characters
{
    public class CharacterEditModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Url]
        [Required]
        public string PictureUrl { get; set; }

        [Required]
        [MinLength(DescriptionMinLength)]
        public string Description { get; set; }

        public int ClassId { get; set; }

        public IEnumerable<ClassViewModel> Classes { get; set; }
    }
}
