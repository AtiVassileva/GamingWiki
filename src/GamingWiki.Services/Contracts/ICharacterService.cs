using GamingWiki.Models;

namespace GamingWiki.Services.Contracts
{
    public interface ICharacterService
    {
        Game ParseGame(string gameName);

        bool GameExists(int gameId);

        bool ClassExists(int classId);
    }
}
