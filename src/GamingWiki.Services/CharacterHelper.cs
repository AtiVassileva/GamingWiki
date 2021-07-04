using System.Linq;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Services.Contracts;

namespace GamingWiki.Services
{
    public class CharacterHelper : ICharacterHelper
    {
        private readonly ApplicationDbContext dbContext;

        public CharacterHelper(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Game ParseGame(string gameName) 
            => this.dbContext.Games.First(g => g.Name == gameName);
    }
}
