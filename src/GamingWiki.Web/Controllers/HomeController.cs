using GamingWiki.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GamingWiki.Services.Contracts;
using GamingWiki.Web.Models.Home;

namespace GamingWiki.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGameService gameService;
        private readonly IArticleService articleService;
        private readonly ITrickService trickService;
        public HomeController(IGameService gameService, IArticleService articleService, ITrickService trickService)
        {
            this.articleService = articleService;
            this.trickService = trickService;
            this.gameService = gameService;
        }

        public IActionResult Index()
        {
            var userIdentity = this.User.Identity;

            if (userIdentity is {IsAuthenticated: false})
            {
                return this.View("GuestPage");
            }
            
            return this.View(new HomeViewModel
            {
                LatestArticles = this.articleService.GetLatest(),
                LatestGames = this.gameService.GetLatest(),
                LatestTricks = this.trickService.GetLatest()
            });
        }

        public IActionResult About() => this.View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
