using System.Collections.Generic;
using GamingWiki.Models;

namespace GamingWiki.Services.Contracts
{
    public interface IGameHelper
    {
        Place ParsePlace(string placeName, string placeType);

        IEnumerable<Creator> ParseCreators(string creatorsNames);

        IDictionary<string, double> GetRatings(int gameId);
    }
}
