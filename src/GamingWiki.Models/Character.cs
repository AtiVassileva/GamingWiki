namespace GamingWiki.Models
{
    public class Character
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Game Game { get; set; }
    }
}
