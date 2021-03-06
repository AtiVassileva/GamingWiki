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
            return RedirectToAction("All", "Characters", 
                new { area = "" });
        }

        public IActionResult Pending() 
            => this.View(this.helper.GetPending());
    }
}
