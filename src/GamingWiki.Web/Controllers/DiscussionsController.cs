using System.Collections.Generic;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Discussions;
using GamingWiki.Web.Infrastructure;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Discussions;
using Microsoft.AspNetCore.Mvc;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Common.ExceptionMessages;

namespace GamingWiki.Web.Controllers
{
    public class DiscussionsController : Controller
    {
        private const int DiscussionsPerPage = 5;

        private readonly IDiscussionService discussionService;

        public DiscussionsController(IDiscussionService discussionService) 
            => this.discussionService = discussionService;

        public IActionResult All(int pageIndex = 1) 
            => this.View(new DiscussionFullModel
            {
                Discussions = PaginatedList<DiscussionAllServiceModel>
                    .Create(this.discussionService.All(),
                        pageIndex, DiscussionsPerPage),
                Tokens = new KeyValuePair<object, object>("All", null)
            });

        public IActionResult Create() => this.View();

        [HttpPost]
        public IActionResult Create(DiscussionFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var creatorId = this.User.GetId();
            var discussionId = this.discussionService.Create(creatorId, model.Description);

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Details(int discussionId)
            => this.discussionService.DiscussionExists(discussionId) ?
                this.View(this.discussionService.Details(discussionId)) :
                this.View("Error", CreateError(NonExistingDiscussionExceptionMessage));
    }
}
