using System.Collections.Generic;
using System.Linq;
using GamingWiki.Data;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Comments;

namespace GamingWiki.Services
{
    public class CommentService : ICommentService
    {
        private const string DateFormat = "dd/MM/yyyy";

        private readonly ApplicationDbContext dbContext;
        private readonly IReplyService replyService;

        public CommentService(ApplicationDbContext dbContext, IReplyService replyService)
        {
            this.dbContext = dbContext;
            this.replyService = replyService;
        }

        public IEnumerable<CommentServiceModel> AllByArticle(int articleId) 
            => this.dbContext.Comments
                .Where(c => c.ArticleId == articleId)
                .Select(c =>
                    new CommentServiceModel
                    {
                        Id = c.Id,
                        Content = c.Content,
                        Commenter = c.Commenter.UserName,
                        AddedOn = c.AddedOn.ToString(DateFormat),
                        Replies = this.replyService.AllByComment(c.Id)
                    }).ToList();
    }
}
