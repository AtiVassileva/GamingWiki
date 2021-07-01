namespace GamingWiki.Models
{
    public class GameCreator
    {
        public int CreatorId { get; set; }

        public Creator Creator { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; }
    }
}
