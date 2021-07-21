using GamingWiki.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GamingWiki.Data;
using GamingWiki.Web.Models.Articles;

namespace GamingWiki.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
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
                .Select(a => new ArticleSimpleModel
                {
                    Id = a.Id,
                    Heading = a.Heading,
                    PictureUrl = a.PictureUrl,
                    PublishedOn = a.PublishedOn.ToString("d")
                }).Take(3).ToList();

            return this.View(latestArticles);
        }

        public IActionResult Privacy() => this.View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
