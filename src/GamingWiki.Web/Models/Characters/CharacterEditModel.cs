using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamingWiki.Web.Models.Classes;
using static GamingWiki.Models.Common.DataConstants;

namespace GamingWiki.Web.Models.Characters
{
    public class CharacterEditModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Url]
        [Required]
        [Display(Name = "Picture URL")]
        public string PictureUrl { get; set; }

        [Required]
        [MinLength(DescriptionMinLength)]
        public string Description { get; set; }

        [Display(Name = "Class")]
        public int ClassId { get; set; }

        public IEnumerable<ClassViewModel> Classes { get; set; }
    }
}
