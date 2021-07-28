using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Characters;
using GamingWiki.Web.Models.Characters;
using static GamingWiki.Web.Common.WebConstants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    public class CharactersController : Controller
    {
        private readonly ICharacterService helper;

        public CharactersController(ICharacterService helper)
            => this.helper = helper;

        [Authorize]
        public IActionResult All()
            => this.View(new CharacterFullModel
            {
                Characters = this.helper.All(),
                Classes = this.helper.GetClasses()
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
                this.ModelState.AddModelError(nameof(model.GameId), "Game does not exist.");
            }

            if (!this.helper.ClassExists(model.ClassId))
            {
                this.ModelState.AddModelError(nameof(model.ClassId), "Class does not exist.");
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

        [Authorize]
        public IActionResult Details(int characterId)
            => this.helper.CharacterExists(characterId) ?
                this.View(this.helper.Details(characterId)) :
                 this.View("Error");

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(int characterId)
        {
            if (!this.helper.CharacterExists(characterId))
            {
                return this.View("Error");
            }

            var dbModel = this.helper.Details(characterId);

            var characterModel = new CharacterServiceEditModel
            {
                Id = dbModel.Id,
                Name = dbModel.Name,
                Class = dbModel.Class,
                ClassId = dbModel.ClassId,
                PictureUrl = dbModel.PictureUrl,
                Description = dbModel.Description,
                Classes = this.helper.GetClasses()
            };

            return this.View(characterModel);
        }

        [HttpPost]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Edit(CharacterServiceEditModel model, int characterId)
        {
            if (!this.helper.ClassExists(model.ClassId))
            {
                this.ModelState.AddModelError(nameof(model.ClassId), "Class does not exist.");
            }

            if (!this.ModelState.IsValid)
            {
                var dbModel = this.helper.Details(characterId);

                model = new CharacterServiceEditModel
                {
                    Id = dbModel.Id,
                    Name = dbModel.Name,
                    Class = dbModel.Class,
                    ClassId = dbModel.ClassId,
                    PictureUrl = dbModel.PictureUrl,
                    Description = dbModel.Description,
                    Classes = this.helper.GetClasses()
                };

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
                return this.View("Error");
            }

            this.helper.Delete(characterId);
            return this.RedirectToAction(nameof(this.All));
        }

        [Authorize]
        public IActionResult Search(string letter)
            => this.View(nameof(this.All), new CharacterFullModel
            {
                Characters = this.helper.Search(letter),
                Classes = this.helper.GetClasses()
            });

        [Authorize]
        public IActionResult Filter(int classId)
            => this.View(nameof(this.All), new CharacterFullModel
            {
                Characters = this.helper.Filter(classId),
                Classes = this.helper.GetClasses()
            });
    }
}
