using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamingWiki.Services.Models.Classes;
using static GamingWiki.Models.Common.DataConstants;

namespace GamingWiki.Services.Models.Characters
{
    public class CharacterServiceEditModel
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

        public IEnumerable<ClassSimpleServiceModel> Classes { get; set; }
    }
}
