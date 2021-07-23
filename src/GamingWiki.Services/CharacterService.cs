using System.Linq;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Services.Contracts;

namespace GamingWiki.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly ApplicationDbContext dbContext;

        public CharacterService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Game ParseGame(string gameName) 
            => this.dbContext.Games.First(g => g.Name == gameName);

        public bool GameExists(int gameId)
            => this.dbContext.Games.Any(g => g.Id == gameId);

        public bool ClassExists(int classId)
            => this.dbContext.Classes.Any(c => c.Id == classId);
    }
}
