using GamingWiki.Services.Contracts;
using GamingWiki.Web.Models.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GamingWiki.Web.Infrastructure.ClaimsPrincipalExtensions;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Common.ExceptionMessages;
using static GamingWiki.Web.Common.AlertMessages;

namespace GamingWiki.Web.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentService commentService;
        private readonly IArticleService articleService;

        public CommentsController(ICommentService helper, 
            IArticleService articleService)
        {
            this.commentService = helper;
            this.articleService = articleService;
        }

        [HttpPost]
        public IActionResult Add(CommentFormModel model, int articleId)
        {
            if (!this.articleService.ArticleExists(articleId))
            {
                return this.View("Error", CreateError(NonExistingArticleExceptionMessage));
            }

            if (!this.ModelState.IsValid)
            {
                TempData[GlobalMessageKey] = UnsuccessfullyAddedCommentMessage;
                TempData[ColorKey] = DangerAlertColor;

                return RedirectToAction(nameof(ArticlesController.Details), "Articles", new { articleId });
            }

            var commenterId = this.User.GetId();
            this.commentService.Add(articleId, model.Content,commenterId);

            TempData[GlobalMessageKey] = SuccessfullyAddedCommentMessage;

            return RedirectToAction(nameof(ArticlesController.Details), "Articles", new { articleId });
        }
        
        public IActionResult Delete(int commentId)
        {
            if (!this.commentService.CommentExists(commentId))
            {
                return this.View("Error", CreateError(NonExistingCommentExceptionMessage));
            }

            var authorId = this.commentService.GetCommentAuthorId(commentId);

            if (!this.User.IsAdmin() && this.User.GetId() != authorId)
            {
                return this.Unauthorized("Forbidden");
            }

            var articleId = this.commentService.GetArticleId(commentId);

            var deleted = this.commentService.Delete(commentId);

            if (!deleted)
            {
                return this.BadRequest();
            }

            TempData[GlobalMessageKey] = DeletedCommentMessage;

            return RedirectToAction(nameof(ArticlesController.Details), "Articles", new { articleId });
        }
    }
}
