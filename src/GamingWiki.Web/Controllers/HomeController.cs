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

        public HomeController(IGameService gameService, IArticleService articleService)
        {
            this.articleService = articleService;
            this.gameService = gameService;
        }

        public IActionResult Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.View("GuestPage");
            }
            
            return this.View(new HomeViewModel
            {
                LatestArticles = this.articleService.GetLatest(),
                BestGames = this.gameService.GetBest()
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
