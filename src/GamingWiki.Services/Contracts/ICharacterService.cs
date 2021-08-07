using System.Collections.Generic;
using System.Linq;
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

        bool CharacterExists(int characterId);

        int Create(string name, string pictureUrl,
            string description, int classId, int gameId);

        CharacterServiceDetailsModel Details(int characterId);

        IQueryable<CharacterAllServiceModel> All();

        bool Edit(int characterId, CharacterServiceEditModel model);

        bool Delete(int characterId);

        string GetContributorId(int characterId);

        void ApproveCharacter(int characterId);

        IQueryable<CharacterAllServiceModel> Mine(string contributorId);

        IQueryable<CharacterAllServiceModel> Search(string letter);

        IQueryable<CharacterAllServiceModel> Filter(int classId);

        IEnumerable<CharacterPendingModel> GetPending();

        IEnumerable<ClassSimpleServiceModel> GetClasses();

        IEnumerable<GameServiceSimpleModel> GetGames();
    }
}
