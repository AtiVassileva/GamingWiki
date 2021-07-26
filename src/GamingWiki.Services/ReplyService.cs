using System.Collections.Generic;
using System.Linq;
using GamingWiki.Data;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Replies;

namespace GamingWiki.Services
{
    public class ReplyService : IReplyService
    {
        private readonly ApplicationDbContext dbContext;

        public ReplyService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<ReplyServiceModel> AllByComment(int commentId) 
            => this.dbContext.Replies
                .Where(r => r.CommentId == commentId)
                .Select(r =>
                    new ReplyServiceModel
                    {
                        Id = r.Id,
                        Content = r.Content,
                        Replier = r.Replier.UserName
                    }).ToList();
    }
}
