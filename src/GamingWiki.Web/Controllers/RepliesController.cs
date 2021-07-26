using GamingWiki.Services.Contracts;
using GamingWiki.Web.Infrastructure;
using GamingWiki.Web.Models.Replies;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
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
                return this.View("Error");
            }

            var replierId = this.User.GetId();
            var articleId = this.helper.Add(model.ReplyContent, commentId, replierId);

           return this.Redirect($"/Articles/Details?articleId={articleId}");
        }

        public IActionResult Delete(int replyId)
        {
            var articleId = this.helper.Delete(replyId);
            return this.Redirect($"/Articles/Details?articleId={articleId}");
        }
    }
}
