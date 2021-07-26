using System.Collections.Generic;
using GamingWiki.Models;
using GamingWiki.Services.Models.Characters;
using GamingWiki.Services.Models.Classes;
using GamingWiki.Services.Models.Games;

namespace GamingWiki.Services.Contracts
{
    public interface ICharacterService
    {
        Game ParseGame(string gameName);

        bool GameExists(int gameId);

        bool ClassExists(int classId);

        int Create(string name, string pictureUrl,
            string description, int classId, int gameId);

        CharacterServiceDetailsModel Details(int characterId);

        IEnumerable<CharacterAllServiceModel> All();

        void Edit(int characterId, CharacterServiceEditModel model);

        void Delete(int characterId);

        IEnumerable<CharacterAllServiceModel> Search(string letter);

        IEnumerable<CharacterAllServiceModel> Filter(int classId);

        IEnumerable<ClassSimpleServiceModel> GetClasses();

        IEnumerable<GameServiceSimpleModel> GetGames();
    }
}
