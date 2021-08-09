using GamingWiki.Services.Contracts;
using GamingWiki.Web.Infrastructure;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Replies;
using static GamingWiki.Web.Common.ExceptionMessages;
using static GamingWiki.Web.Common.AlertMessages;
using static GamingWiki.Web.Common.WebConstants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    [Authorize]
    public class RepliesController : Controller
    {
        private readonly IReplyService replyService;
        private readonly ICommentService commentService;

        public RepliesController(IReplyService helper, ICommentService commentService)
        {
            this.replyService = helper;
            this.commentService = commentService;
        }

        [HttpPost]
        public IActionResult Add(ReplyFormModel model, int commentId)
        {
            if (!this.commentService.CommentExists(commentId))
            {
                return this.View("Error", CreateError(NonExistingCommentExceptionMessage));
            }

            var articleId = this.commentService.GetArticleId(commentId);

            if (!this.ModelState.IsValid)
            {
                TempData[GlobalMessageKey] = UnsuccessfullyAddedReplyMessage;
                TempData[ColorKey] = DangerAlertColor;

                return RedirectToAction(nameof(ArticlesController.Details), "Articles", new { articleId });
            }

            var replierId = this.User.GetId();
            this.replyService.Add(model.ReplyContent, commentId, replierId);

            TempData[GlobalMessageKey] = SuccessfullyAddedReplyMessage;

            return RedirectToAction(nameof(ArticlesController.Details), "Articles", new { articleId });
        }
        
        public IActionResult Delete(int replyId)
        {
            if (!this.replyService.ReplyExists(replyId))
            {
                return this.View("Error", CreateError(NonExistingReplyExceptionMessage));
            }

            var replierId = this.replyService.GetReplyAuthorId(replyId);

            if (!this.User.IsAdmin() && this.User.GetId() != replierId)
            {
                return this.Unauthorized();
            }

            var articleId = this.replyService.GetArticleIdByReply(replyId);

            var deleted = this.replyService.Delete(replyId);

            if (!deleted)
            {
                return this.BadRequest();
            }

            TempData[GlobalMessageKey] = DeletedReplyMessage;

            return RedirectToAction(nameof(ArticlesController.Details), "Articles", new { articleId });
        }
    }
}