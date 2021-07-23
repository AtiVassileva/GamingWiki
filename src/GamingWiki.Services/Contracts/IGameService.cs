using System.Collections.Generic;
using GamingWiki.Models;

namespace GamingWiki.Services.Contracts
{
    public interface IGameService
    {
        IEnumerable<Creator> ParseCreators(string creatorsNames);

        IDictionary<string, double> GetRatings(int gameId);

        bool AreaExists(int areaId);

        bool GenreExists(int genreId);
    }
}
