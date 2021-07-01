using GamingWiki.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class Game
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public Place Place { get; set; }

        public ICollection<Character> Characters { get; set; }
        = new HashSet<Character>();

    }
}
