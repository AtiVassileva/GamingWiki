using GamingWiki.Services.Contracts;
using GamingWiki.Web.Models.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GamingWiki.Web.Infrastructure.ClaimsPrincipalExtensions;

namespace GamingWiki.Web.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentService helper;

        public CommentsController(ICommentService helper) 
            => this.helper = helper;

        [HttpPost]
        [Authorize]
        public IActionResult Add(CommentFormModel model, int articleId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var commenterId = this.User.GetId();
            this.helper.Add(articleId, model.Content,commenterId);

            return this.Redirect($"/Articles/Details?articleId={articleId}");
        }

        public IActionResult Delete(int commentId)
        {
            var articleId = this.helper.Delete(commentId);
            return this.Redirect($"/Articles/Details?articleId={articleId}");
        }
    }
}
