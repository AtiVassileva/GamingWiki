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

        bool TrickExists(int trickId);

        void Edit(int trickId, string heading, string content, string pictureUrl);

        void Delete(int trickId);

        IEnumerable<TrickServiceListingModel> Search(string searchCriteria);

        string GetTrickAuthorId(int trickId);

        TrickServiceListingModel Details(int trickId);

        IEnumerable<GameServiceSimpleModel> GetGames();

        IEnumerable<TrickServiceHomeModel> GetLatest();
    }
}
