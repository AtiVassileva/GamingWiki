using GamingWiki.Services.Contracts;
using GamingWiki.Web.Models.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GamingWiki.Web.Infrastructure.ClaimsPrincipalExtensions;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Common.ExceptionMessages;

namespace GamingWiki.Web.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentService helper;

        public CommentsController(ICommentService helper) 
            => this.helper = helper;

        [HttpPost]
        public IActionResult Add(CommentFormModel model, int articleId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Error");
            }

            var commenterId = this.User.GetId();
            this.helper.Add(articleId, model.Content,commenterId);

            return this.Redirect($"/Articles/Details?articleId={articleId}");
        }
        
        public IActionResult Delete(int commentId)
        {
            if (!this.helper.CommentExists(commentId))
            {
                return this.View("Error", CreateError(NonExistingCommentExceptionMessage));
            }

            var authorId = this.helper.GetCommentAuthorId(commentId);

            if (!this.User.IsAdmin() || this.User.GetId() != authorId)
            {
                return this.Unauthorized();
            }

            var articleId = this.helper.Delete(commentId);

            return this.Redirect($"/Articles/Details?articleId={articleId}");
        }
    }
}
