using System.Collections.Generic;
using GamingWiki.Services.Models.Areas;
using GamingWiki.Services.Models.Games;
using GamingWiki.Services.Models.Genres;

namespace GamingWiki.Services.Contracts
{
    public interface IGameService
    {
        bool AreaExists(int areaId);

        bool GenreExists(int genreId);

        int Create(string name, string pictureUrl, string trailerUrl, string description, int areaId, int genreId, string creatorsNames);

        IEnumerable<GameServiceListingModel> All();

        GameServiceDetailsModel Details(int gameId);

        void Edit(int gameId, string description, string pictureUrl, int areaId, string trailerUrl);

        void Delete(int gameId);

        IEnumerable<GameServiceListingModel> Search(string letter);

        IEnumerable<GameServiceListingModel> Filter(int genreId);

        IEnumerable<AreaServiceModel> GetAreas();

        IEnumerable<GenreServiceModel> GetGenres();

        IEnumerable<GameServiceHomeModel> GetLatest();
    }
}
