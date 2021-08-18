using System.Collections.Generic;
using AutoMapper;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Characters;
using GamingWiki.Web.Infrastructure;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Characters;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Common.ExceptionMessages;
using static GamingWiki.Web.Common.AlertMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    [Authorize]
    public class CharactersController : Controller
    {
        private const int CharactersPerPage = 3;

        private readonly ICharacterService characterService;
        private readonly IMapper mapper;

        public CharactersController(ICharacterService helper, 
            IMapper mapper)
        {
            this.characterService = helper;
            this.mapper = mapper;
        }

        public IActionResult All(int pageIndex = 1)
            => this.View(new CharacterFullModel
            {
                Characters = PaginatedList<CharacterAllServiceModel>
                    .Create(this.characterService.All(approvedOnly: 
                            !this.User.IsAdmin()),
                        pageIndex, CharactersPerPage),
                Classes = this.characterService.GetClasses(),
                Tokens = new KeyValuePair<object, object>("All", null)
            });
        
        public IActionResult Create()
            => this.View(new CharacterFormModel
            {
                Classes = this.characterService.GetClasses(),
                Games = this.characterService.GetGames()
            });

        [HttpPost]
        public IActionResult Create(CharacterFormModel model)
        {
            if (!this.characterService.GameExists(model.GameId))
            {
                this.ModelState.AddModelError(nameof(model.GameId), NonExistingGameExceptionMessage);
            }

            if (!this.characterService.ClassExists(model.ClassId))
            {
                this.ModelState.AddModelError(nameof(model.ClassId), NonExistingClassExceptionMessage);
            }

            if (!this.ModelState.IsValid)
            {
                model.Games = this.characterService.GetGames();
                model.Classes = this.characterService.GetClasses();

                return this.View(model);
            }

            var characterId = this.characterService.Create(model.Name, model.PictureUrl,
                model.Description, model.ClassId, model.GameId,
                isApproved: this.User.IsAdmin(), contributorId:this.User.GetId());

            TempData[GlobalMessageKey] = this.User.IsAdmin() 
                ? SuccessfullyAddedCharacterAdminMessage 
                    : SuccessfullyAddedCharacterUserMessage;

            return this.RedirectToAction(nameof(this.Details),
                new {characterId});
        }
        
        public IActionResult Details(int characterId)
            => this.characterService.CharacterExists(characterId) ?
                this.View(this.characterService.Details(characterId)) :
                 this.View("Error", CreateError(NonExistingCharacterExceptionMessage));
        
        public IActionResult Edit(int characterId)
        {
            if (!this.characterService.CharacterExists(characterId))
            {
                return this.View("Error", CreateError(NonExistingCharacterExceptionMessage));
            }

            var contributorId = this.characterService.GetContributorId(characterId);

            if (!this.User.IsAdmin() && this.User.GetId() != contributorId)
            {
                return this.Unauthorized();
            }

            var dbModel = this.characterService.Details(characterId);

            var characterModel = this.mapper
                .Map<CharacterServiceEditModel>(dbModel);

            characterModel.Classes = this.characterService.GetClasses();

            return this.View(characterModel);
        }

        [HttpPost]
        public IActionResult Edit(CharacterServiceEditModel model, int characterId)
        {
            if (!this.characterService.ClassExists(model.ClassId))
            {
                this.ModelState.AddModelError(nameof(model.ClassId), NonExistingClassExceptionMessage);
            }

            if (!this.ModelState.IsValid)
            {
                var dbModel = this.characterService.Details(characterId);

                model = this.mapper
                    .Map<CharacterServiceEditModel>(dbModel);
                
                model.Classes = this.characterService.GetClasses();

                return this.View(model);
            }

            model.IsApproved = this.User.IsAdmin();

            var edited = this.characterService.Edit(characterId, model);

            if (!edited)
            {
                return this.BadRequest();
            }

            TempData[GlobalMessageKey] = this.User.IsAdmin()
                ? SuccessfullyEditedCharacterAdminMessage
                : SuccessfullyEditedCharacterUserMessage;

            return this.RedirectToAction(nameof(this.Details),
                new { characterId });
        }
        
        public IActionResult Delete(int characterId)
        {
            if (!this.characterService.CharacterExists(characterId))
            {
                return this.View("Error", CreateError(NonExistingCharacterExceptionMessage));
            }

            var contributorId = this.characterService.GetContributorId(characterId);

            if (!this.User.IsAdmin() && this.User.GetId() != contributorId)
            {
                return this.Unauthorized();
            }

            var deleted = this.characterService.Delete(characterId);

            if (!deleted)
            {
                return this.BadRequest();
            }

            TempData[GlobalMessageKey] = DeletedCharacterMessage;

            return this.RedirectToAction(nameof(this.All));
        }
        
        public IActionResult Search([FromQuery(Name = "parameter")] string letter, int pageIndex = 1)
            => this.View(nameof(this.All), new CharacterFullModel
            {
                Characters = PaginatedList<CharacterAllServiceModel>
                    .Create(this.characterService.Search(letter),
                        pageIndex, CharactersPerPage),
                Classes = this.characterService.GetClasses(),
                Tokens = new KeyValuePair<object, object>("Search", letter)
            });
        
        public IActionResult Filter([FromQuery(Name = "parameter")]
            int classId, int pageIndex = 1)
            => this.View(nameof(this.All), new CharacterFullModel
            {
                Characters = PaginatedList<CharacterAllServiceModel>
                    .Create(this.characterService.Filter(classId),
                        pageIndex, CharactersPerPage),
                Classes = this.characterService.GetClasses(),
                Tokens = new KeyValuePair<object, object>("Filter", classId)
            });

        public IActionResult Mine(int pageIndex = 1)
            => this.View(nameof(this.All), new CharacterFullModel
            {
                Characters = PaginatedList<CharacterAllServiceModel>
                    .Create(this.characterService.Mine(this.User.GetId()),
                        pageIndex, CharactersPerPage),
                Classes = this.characterService.GetClasses(),
                Tokens = new KeyValuePair<object, object>("Mine", null)
            });
    }
}