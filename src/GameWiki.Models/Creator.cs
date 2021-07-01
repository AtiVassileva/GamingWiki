namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class Creator
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<GameCreator> GamesCreators { get; set; }
        = new HashSet<GameCreator>();
    }
}
