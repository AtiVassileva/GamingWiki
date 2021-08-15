using GamingWiki.Services.Contracts;
using GamingWiki.Web.Infrastructure;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Common.ExceptionMessages;
using static GamingWiki.Web.Common.AlertMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly IMessageService messageService;

        public MessagesController(IMessageService messageService) 
            => this.messageService = messageService;

        public IActionResult Delete(int messageId)
        {
            if (!this.messageService.MessageExists(messageId))
            {
                return this.View("Error", CreateError(NonExistingMessageExceptionMessage));
            }

            var senderId = this.messageService.GetSenderId(messageId);

            if (!this.User.IsAdmin() && this.User.GetId() != senderId)
            {
                return this.Unauthorized();
            }

            var discussionId = this.messageService
                .GetDiscussionId(messageId);

            var deleted = this.messageService.Delete(messageId);

            if (!deleted)
            {
                return this.BadRequest();
            }

            TempData[GlobalMessageKey] = DeletedMessageAlertMessage;

            return RedirectToAction(nameof(DiscussionsController.Chat), "Discussions", new { discussionId });
        }
    }
}
