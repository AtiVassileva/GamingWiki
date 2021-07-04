using GamingWiki.Models;

namespace GamingWiki.Services.Contracts
{
    public interface ICharacterHelper
    {
        Game ParseGame(string gameName);
    }
}
