using GamingWiki.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Areas.Admin.Controllers
{
    public class GamesController : AdminController
    {
        private readonly IGameService helper;

        public GamesController(IGameService helper) 
            => this.helper = helper;

        public IActionResult Approve(int gameId)
        {
            this.helper.Approve(gameId);
            return RedirectToAction("All", "Games", new { area = "" });
        }

        public IActionResult Pending() 
            => this.View(this.helper.GetPending());
    }
}
