using System.Collections.Generic;
using AutoMapper;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Discussions;
using GamingWiki.Web.Infrastructure;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Discussions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Common.ExceptionMessages;
using static GamingWiki.Web.Common.AlertMessages;

namespace GamingWiki.Web.Controllers
{
    [Authorize]
    public class DiscussionsController : Controller
    {
        private const int DiscussionsPerPage = 5;

        private readonly IDiscussionService discussionService;
        private readonly IMapper mapper;

        public DiscussionsController(IDiscussionService discussionService, IMapper mapper)
        {
            this.discussionService = discussionService;
            this.mapper = mapper;
        }

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
            var discussionId = this.discussionService.Create(creatorId, model.Name, model.Description, model.PictureUrl, 
                model.MembersLimit);

            TempData[GlobalMessageKey] = SuccessfullyAddedDiscussionMessage;

            return this.RedirectToAction(nameof(this.Details),
                new { discussionId });
        }

        public IActionResult Details(int discussionId)
        {
            if (!this.discussionService.DiscussionExists(discussionId))
            {
                return this.View("Error", CreateError(NonExistingDiscussionExceptionMessage));
            }

            ViewBag.IsUserMemberOfDiscussion = this.discussionService
                .UserParticipatesInDiscussion(discussionId, this.User.GetId());

            return this.View(this.discussionService
                .Details(discussionId));
        }

        public IActionResult Edit(int discussionId)
        {
            if (!this.discussionService.DiscussionExists(discussionId))
            {
                return this.View("Error", CreateError(NonExistingDiscussionExceptionMessage));
            }

            var creatorId = this.discussionService
                .GetCreatorId(discussionId);

            if (!this.User.IsAdmin() && this.User.GetId() != creatorId)
            {
                return this.Unauthorized();
            }

            var detailsModel = this.discussionService
                .Details(discussionId);

            var discussionModel = this.mapper
                .Map<DiscussionServiceEditModel>(detailsModel);

            return this.View(discussionModel);
        }

        [HttpPost]
        public IActionResult Edit(DiscussionServiceEditModel model, int discussionId)
        {
            if (!this.ModelState.IsValid)
            {
                var dbModel = this.discussionService.Details(discussionId);

                model = this.mapper
                    .Map<DiscussionServiceEditModel>(dbModel);
                
                return this.View(model);
            }
            
            var edited = this.discussionService.Edit(discussionId, model);

            if (!edited)
            {
                return this.BadRequest();
            }

            TempData[GlobalMessageKey] =  SuccessfullyEditedDiscussionMessage;

            return this.RedirectToAction(nameof(this.Details),
                new { discussionId });
        }

        public IActionResult Delete(int discussionId)
        {
            if (!this.discussionService.DiscussionExists(discussionId))
            {
                return this.View("Error", CreateError(NonExistingDiscussionExceptionMessage));
            }

            var creatorId = this.discussionService
                .GetCreatorId(discussionId);

            if (!this.User.IsAdmin() && this.User.GetId() != creatorId)
            {
                return this.Unauthorized();
            }

            var deleted = this.discussionService.Delete(discussionId);

            if (!deleted)
            {
                return this.BadRequest();
            }

            TempData[GlobalMessageKey] = DeletedDiscussionMessage;

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Search(string parameter, int pageIndex = 1, string name = null)
            => this.View(nameof(this.All), new DiscussionFullModel
            {
                Discussions = PaginatedList<DiscussionAllServiceModel>
                    .Create(this.discussionService.Search(parameter),
                        pageIndex, DiscussionsPerPage),
                Tokens = new KeyValuePair<object, object>("Search", parameter)
            });

        [HttpPost]
        public IActionResult Search(string searchCriteria,
            int pageIndex = 1)
            => this.View(nameof(this.All), new DiscussionFullModel
            {
                Discussions = PaginatedList<DiscussionAllServiceModel>
                    .Create(this.discussionService.Search(searchCriteria),
                        pageIndex, DiscussionsPerPage),
                Tokens = new KeyValuePair<object, object>("Search", searchCriteria)
            });

        public IActionResult Join(int discussionId)
        {
            if (!this.discussionService.DiscussionExists(discussionId))
            {
                return this.View("Error", CreateError(NonExistingDiscussionExceptionMessage));
            }
            
            if (this.discussionService.DiscussionFull(discussionId))
            {
                TempData[GlobalMessageKey] = FullDiscussionExceptionMessage;
                TempData[ColorKey] = "danger";

                return RedirectToAction(nameof(this.Details), discussionId);
            }

            var userId = this.User.GetId();

            if (this.discussionService.UserParticipatesInDiscussion(discussionId, userId))
            {
                TempData[GlobalMessageKey] = "You are already a member of this discussion";
                return RedirectToAction(nameof(this.Chat), discussionId);
            }

            this.discussionService.JoinUserToDiscussion(discussionId, userId: this.User.GetId());

            TempData[GlobalMessageKey] = SuccessfullyJoinedDiscussionMessage;

            return this.RedirectToAction(nameof(this.Chat),
                new { discussionId });
        }

        public IActionResult Leave(int discussionId)
        {
            if (!this.discussionService.DiscussionExists(discussionId))
            {
                return this.View("Error", CreateError(NonExistingDiscussionExceptionMessage));
            }

            var userId = this.User.GetId();

            if (!this.discussionService.UserParticipatesInDiscussion(discussionId, userId))
            {
                TempData[GlobalMessageKey] = NotDiscussionMemberExceptionMessage;
                TempData[ColorKey] = "danger";

                return RedirectToAction(nameof(this.Details), discussionId);
            }

            this.discussionService.RemoveUserFromDiscussion(discussionId, userId);

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Chat(int discussionId)
        {
            if (!this.discussionService.DiscussionExists(discussionId))
            {
                return this.View("Error", CreateError(NonExistingDiscussionExceptionMessage));
            }

            var detailsModel = this.discussionService
                .Details(discussionId);

            var discussionChatModel = this.mapper
                .Map<DiscussionChatServiceModel>(detailsModel);

            discussionChatModel.Messages = this.discussionService
                .GetMessagesForDiscussion(discussionId);

            return this.View(discussionChatModel);
        }

        public IActionResult Mine(int pageIndex = 1)
            => this.View(nameof(this.All), new DiscussionFullModel
            {
                Discussions = PaginatedList<DiscussionAllServiceModel>
                    .Create(this.discussionService
                            .GetDiscussionsByUser(this.User.GetId()),
                        pageIndex, DiscussionsPerPage),
                Tokens = new KeyValuePair<object, object>("Mine", null)
            });
    }
}
