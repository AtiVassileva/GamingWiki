using GamingWiki.Services.Contracts;
using GamingWiki.Web.Infrastructure;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Replies;
using static GamingWiki.Web.Common.ExceptionMessages;
using static GamingWiki.Web.Common.WebConstants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    [Authorize]
    public class RepliesController : Controller
    {
        private readonly IReplyService helper;

        public RepliesController(IReplyService helper)
        => this.helper = helper;

        [HttpPost]
        public IActionResult Add(ReplyFormModel model, int commentId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Error", new ErrorViewModel());
            }

            var replierId = this.User.GetId();
            var articleId = this.helper.Add(model.ReplyContent, commentId, replierId);

            return RedirectToAction(nameof(ArticlesController.Details), "Articles", new { articleId });
        }
        
        public IActionResult Delete(int replyId)
        {
            if (!this.helper.ReplyExists(replyId))
            {
                return this.View("Error", CreateError(NonExistingReplyExceptionMessage));
            }

            var replierId = this.helper.GetReplyAuthorId(replyId);

            if (!this.User.IsAdmin() && this.User.GetId() != replierId)
            {
                return this.Unauthorized();
            }

            var deleted = this.helper.Delete(replyId);

            if (!deleted)
            {
                return this.BadRequest();
            }

            var articleId = this.helper.GetArticleIdByReply(replyId);

            return RedirectToAction(nameof(ArticlesController.Details), "Articles", new { articleId });
        }
    }
}
