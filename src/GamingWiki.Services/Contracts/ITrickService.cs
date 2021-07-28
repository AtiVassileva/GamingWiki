using System.Collections.Generic;
using GamingWiki.Services.Models.Games;
using GamingWiki.Services.Models.Tricks;

namespace GamingWiki.Services.Contracts
{
    public interface ITrickService
    {
        IEnumerable<TrickServiceListingModel> All();

        void Create(string heading, string content, string authorId, string pictureUrl, int gameId);

        bool GameExists(int gameId);

        IEnumerable<GameServiceSimpleModel> GetGames();

        IEnumerable<TrickServiceHomeModel> GetLatest();
    }
}
