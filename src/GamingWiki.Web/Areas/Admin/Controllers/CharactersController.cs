using GamingWiki.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Areas.Admin.Controllers
{
    public class CharactersController : AdminController
    {
        private readonly ICharacterService helper;

        public CharactersController(ICharacterService helper) 
            => this.helper = helper;

        public IActionResult Approve(int characterId)
        {
            this.helper.ApproveCharacter(characterId);
            return this.RedirectToAction("All");
        }

        public IActionResult Pending()
        {
            var pendingCharacters = this.helper.GetPending();
            return this.View(pendingCharacters);
        }
    }
}
