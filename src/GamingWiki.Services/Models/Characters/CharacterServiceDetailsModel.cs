namespace GamingWiki.Services.Models.Characters
{
    public class CharacterServiceDetailsModel : CharacterAllServiceModel
    {
        public string Description { get; set; }

        public string ClassName { get; set; }

        public int ClassId { get; set; }

        public string GameName { get; set; }

        public int GameId { get; set; }

        public string ContributorId { get; set; }
    }
}
