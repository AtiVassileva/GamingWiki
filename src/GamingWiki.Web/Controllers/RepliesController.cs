using System;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Replies;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    public class RepliesController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public RepliesController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpPost]
        public IActionResult Add(ReplyFormModel model, int commentId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Error", new ErrorViewModel
                {
                    RequestId = Guid.NewGuid().ToString()
                });
            }

            var replyDto = new ReplyDtoModel
            {
                Content = model.ReplyContent,
                CommentId = commentId,
                ReplierId = this.User.FindFirstValue(ClaimTypes.NameIdentifier),
                AddedOn = DateTime.UtcNow
            };

            var reply = this.mapper.Map<Reply>(replyDto);

            this.dbContext.Replies.Add(reply);
            this.dbContext.SaveChanges();

            var articleId = this.dbContext.Comments
                .Where(c => c.Id == commentId)
                .Select(c => c.ArticleId)
                .FirstOrDefault();

           return this.Redirect($"/Articles/Details?articleId={articleId}");
        }

        public IActionResult Delete(int replyId)
        {
            var reply = this.dbContext.Replies
                .First(r => r.Id == replyId);

            var articleId = this.dbContext.Replies
                .Where(r => r.Id == replyId)
                .Select(r => r.Comment.ArticleId)
                .FirstOrDefault();

            this.dbContext.Replies.Remove(reply);
            this.dbContext.SaveChanges();

            return this.Redirect($"/Articles/Details?articleId={articleId}");
        }
    }
}
