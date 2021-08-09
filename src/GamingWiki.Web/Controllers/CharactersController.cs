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

        private readonly ICharacterService helper;
        private readonly IMapper mapper;

        public CharactersController(ICharacterService helper, 
            IMapper mapper)
        {
            this.helper = helper;
            this.mapper = mapper;
        }

        public IActionResult All(int pageIndex = 1)
            => this.View(new CharacterFullModel
            {
                Characters = PaginatedList<CharacterAllServiceModel>
                    .Create(this.helper.All(approvedOnly: 
                            !this.User.IsAdmin()),
                        pageIndex, CharactersPerPage),
                Classes = this.helper.GetClasses(),
                Tokens = new KeyValuePair<object, object>("All", null)
            });
        
        public IActionResult Create()
            => this.View(new CharacterFormModel
            {
                Classes = this.helper.GetClasses(),
                Games = this.helper.GetGames()
            });

        [HttpPost]
        public IActionResult Create(CharacterFormModel model)
        {
            if (!this.helper.GameExists(model.GameId))
            {
                this.ModelState.AddModelError(nameof(model.GameId), NonExistingGameExceptionMessage);
            }

            if (!this.helper.ClassExists(model.ClassId))
            {
                this.ModelState.AddModelError(nameof(model.ClassId), NonExistingClassExceptionMessage);
            }

            if (!this.ModelState.IsValid)
            {
                model.Games = this.helper.GetGames();
                model.Classes = this.helper.GetClasses();

                return this.View(model);
            }

            var characterId = this.helper.Create(model.Name, model.PictureUrl,
                model.Description, model.ClassId, model.GameId,
                isApproved: this.User.IsAdmin(), contributorId:this.User.GetId());

            TempData[GlobalMessageKey] = this.User.IsAdmin() 
                ? SuccessfullyAddedCharacterAdminMessage 
                    : SuccessfullyAddedCharacterUserMessage;

            return this.RedirectToAction(nameof(this.Details),
                new {characterId});
        }
        
        public IActionResult Details(int characterId)
            => this.helper.CharacterExists(characterId) ?
                this.View(this.helper.Details(characterId)) :
                 this.View("Error", CreateError(NonExistingCharacterExceptionMessage));
        
        public IActionResult Edit(int characterId)
        {
            if (!this.helper.CharacterExists(characterId))
            {
                return this.View("Error", CreateError(NonExistingGameExceptionMessage));
            }

            var contributorId = this.helper.GetContributorId(characterId);

            if (!this.User.IsAdmin() && this.User.GetId() != contributorId)
            {
                return this.Unauthorized();
            }

            var dbModel = this.helper.Details(characterId);

            var characterModel = this.mapper
                .Map<CharacterServiceEditModel>(dbModel);

            characterModel.Classes = this.helper.GetClasses();

            return this.View(characterModel);
        }

        [HttpPost]
        public IActionResult Edit(CharacterServiceEditModel model, int characterId)
        {
            if (!this.helper.ClassExists(model.ClassId))
            {
                this.ModelState.AddModelError(nameof(model.ClassId), NonExistingClassExceptionMessage);
            }

            if (!this.ModelState.IsValid)
            {
                var dbModel = this.helper.Details(characterId);

                model = this.mapper
                    .Map<CharacterServiceEditModel>(dbModel);
                
                model.Classes = this.helper.GetClasses();

                return this.View(model);
            }

            model.IsApproved = this.User.IsAdmin();

            var edited = this.helper.Edit(characterId, model);

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
            if (!this.helper.CharacterExists(characterId))
            {
                return this.View("Error", CreateError(NonExistingCharacterExceptionMessage));
            }

            var contributorId = this.helper.GetContributorId(characterId);

            if (!this.User.IsAdmin() && this.User.GetId() != contributorId)
            {
                return this.Unauthorized();
            }

            var deleted = this.helper.Delete(characterId);

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
                    .Create(this.helper.Search(letter),
                        pageIndex, CharactersPerPage),
                Classes = this.helper.GetClasses(),
                Tokens = new KeyValuePair<object, object>("Search", letter)
            });
        
        public IActionResult Filter([FromQuery(Name = "parameter")]
            int classId, int pageIndex = 1)
            => this.View(nameof(this.All), new CharacterFullModel
            {
                Characters = PaginatedList<CharacterAllServiceModel>
                    .Create(this.helper.Filter(classId),
                        pageIndex, CharactersPerPage),
                Classes = this.helper.GetClasses(),
                Tokens = new KeyValuePair<object, object>("Filter", classId)
            });

        public IActionResult Mine(int pageIndex = 1)
            => this.View(nameof(this.All), new CharacterFullModel
            {
                Characters = PaginatedList<CharacterAllServiceModel>
                    .Create(this.helper.Mine(this.User.GetId()),
                        pageIndex, CharactersPerPage),
                Classes = this.helper.GetClasses(),
                Tokens = new KeyValuePair<object, object>("Mine", null)
            });

        private void CreateAlertMessage(string message, string color)
        {
            TempData[GlobalMessageKey] = message;
            TempData[ColorKey] = color;
        }
    }
}
