namespace GamingWiki.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using static Common.DataConstants;
    public class Creator
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(CreatorNameMaxLength)]
        public string Name { get; set; }

        public ICollection<GameCreator> GamesCreators { get; set; }
        = new HashSet<GameCreator>();
    }
}
