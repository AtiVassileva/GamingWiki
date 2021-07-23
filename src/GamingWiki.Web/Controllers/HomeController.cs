using GamingWiki.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using GamingWiki.Data;
using GamingWiki.Services;
using GamingWiki.Services.Contracts;
using GamingWiki.Web.Models.Articles;
using GamingWiki.Web.Models.Games;
using GamingWiki.Web.Models.Home;

namespace GamingWiki.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IGameService gameHelper;

        public HomeController(ApplicationDbContext dbContext, IGameService gameHelper)
        {
            this.dbContext = dbContext;
            this.gameHelper = new GameService(this.dbContext);
        }

        public IActionResult Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.View("GuestPage");
            }

            var latestArticles = this.dbContext
                .Articles
                .OrderByDescending(a => a.Id)
                .Select(a => new ArticleHomeModel
                {
                    Id = a.Id,
                    Heading = a.Heading,
                    PictureUrl = a.PictureUrl,
                    ShortContent = a.Content.Substring(0, 200)
                }).Take(3).ToList();

            var bestGames = this.dbContext.Games
                .Select(g => new GameHomeModel
                {
                    Id = g.Id,
                    Name = g.Name.ToUpper(),
                    PictureUrl = g.PictureUrl,
                    Rating = this.gameHelper.GetRatings(g.Id).Values.Average()
                })
                .ToList()
                .OrderByDescending(g => g.Rating)
                .Take(3).ToList();

            return this.View(new HomeViewModel
            {
                LatestArticles = latestArticles,
                BestGames = bestGames
            });
        }

        public IActionResult Privacy() => this.View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
