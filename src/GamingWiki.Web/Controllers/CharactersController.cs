﻿using System.Linq;
using AutoMapper;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Characters;
using GamingWiki.Web.Models.Characters;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Common.ExceptionMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    [Authorize]
    public class CharactersController : Controller
    {
        private readonly ICharacterService helper;
        private readonly IMapper mapper;

        public CharactersController(ICharacterService helper, 
            IMapper mapper)
        {
            this.helper = helper;
            this.mapper = mapper;
        }

        public IActionResult All()
            => this.View(new CharacterFullModel
            {
                Characters = this.helper.All().OrderBy(c => c.Name),
                Classes = this.helper.GetClasses().OrderBy(c => c.Name)
            });

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Create()
            => this.View(new CharacterFormModel
            {
                Classes = this.helper.GetClasses(),
                Games = this.helper.GetGames()
            });

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
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
                model.Description, model.ClassId, model.GameId);

            return this.RedirectToAction(nameof(this.Details),
                new { characterId = $"{characterId}" });
        }
        
        public IActionResult Details(int characterId)
            => this.helper.CharacterExists(characterId) ?
                this.View(this.helper.Details(characterId)) :
                 this.View("Error", CreateError(NonExistingGameExceptionMessage));

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int characterId)
        {
            if (!this.helper.CharacterExists(characterId))
            {
                return this.View("Error", CreateError(NonExistingGameExceptionMessage));
            }

            var dbModel = this.helper.Details(characterId);

            var characterModel = this.mapper
                .Map<CharacterServiceEditModel>(dbModel);

            characterModel.Classes = this.helper.GetClasses()
                .OrderBy(c => c.Name);

            return this.View(characterModel);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
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

                model.Classes = this.helper.GetClasses()
                    .OrderBy(c => c.Name);

                return this.View(model);
            }

            this.helper.Edit(characterId, model);

            return this.RedirectToAction(nameof(this.Details),
                new { characterId = $"{characterId}" });
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Delete(int characterId)
        {
            if (!this.helper.CharacterExists(characterId))
            {
                return this.View("Error", CreateError(NonExistingCharacterExceptionMessage));
            }

            this.helper.Delete(characterId);
            return this.RedirectToAction(nameof(this.All));
        }
        
        public IActionResult Search(string letter)
            => this.View(nameof(this.All), new CharacterFullModel
            {
                Characters = this.helper.Search(letter)
                    .OrderBy(c => c.Name),
                Classes = this.helper.GetClasses().OrderBy(c => c.Name)
            });
        
        public IActionResult Filter(int classId)
            => this.View(nameof(this.All), new CharacterFullModel
            {
                Characters = this.helper.Filter(classId)
                    .OrderBy(c => c.Name),
                Classes = this.helper.GetClasses().OrderBy(c => c.Name)
            });
    }
}
