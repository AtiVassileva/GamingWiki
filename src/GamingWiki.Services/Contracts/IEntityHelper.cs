using System.Collections.Generic;
using GamingWiki.Models;

namespace GamingWiki.Services.Contracts
{
    public interface IEntityHelper
    {
        Place ParsePlace(string placeName, string placeType);

        IEnumerable<Creator> ParseCreators(string creatorsNames);
    }
}
