using System.Collections.Generic;
using System.Linq;
using GamingWiki.Services.Models.Areas;
using GamingWiki.Services.Models.Games;
using GamingWiki.Services.Models.Genres;
using GamingWiki.Services.Models.Platforms;
using GamingWiki.Services.Models.Reviews;

namespace GamingWiki.Services.Contracts
{
    public interface IGameService
    {
        bool AreaExists(int areaId);

        bool GenreExists(int genreId);

        bool GameExists(int gameId);

        bool GameExists(string gameName);

        int Create(string name, string pictureUrl, string trailerUrl, string description, int areaId, int genreId, string creatorsNames, IEnumerable<int> supportedPlatforms);

        IQueryable<GameServiceListingModel> All();

        GameServiceDetailsModel Details(int gameId);

        bool Edit(int gameId, string description, string pictureUrl, int areaId, string trailerUrl, IEnumerable<int> platforms);

        bool Delete(int gameId);

        IQueryable<GameServiceListingModel> Search(string letter);

        IQueryable<GameServiceListingModel> Filter(int genreId);

        IEnumerable<AreaServiceModel> GetAreas();

        IEnumerable<GenreServiceModel> GetGenres();

        IEnumerable<PlatformServiceModel> GetPlatforms();

        IEnumerable<int> GetGamePlatforms(int gameId);

        IEnumerable<GameServiceListingModel> GetLatest();

        IEnumerable<ReviewServiceSimpleModel> GetReviews(int gameId);
    }
}
