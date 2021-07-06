using System;
using System.Security.Claims;
using AutoMapper;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Comments;
using Microsoft.AspNetCore.Mvc;

namespace GamingWiki.Web.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public CommentsController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpPost]
        public IActionResult Add(CommentFormModel model, int articleId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Error", new ErrorViewModel
                {
                    RequestId = Guid.NewGuid().ToString()
                });
            }

            var commentDto = new CommentDtoModel
            {
                Content = model.Content,
                ArticleId = articleId,
                CommenterId = this.User.FindFirstValue(ClaimTypes.NameIdentifier),
                AddedOn = DateTime.UtcNow
            };

            var comment = this.mapper.Map<Comment>(commentDto);

            this.dbContext.Comments.Add(comment);
            this.dbContext.SaveChanges();

            return this.Redirect($"/Articles/Details?articleId={articleId}");
        }
    }
}
