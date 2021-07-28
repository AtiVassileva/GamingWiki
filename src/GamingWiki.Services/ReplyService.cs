using System;
using System.Collections.Generic;
using System.Linq;
using GamingWiki.Data;
using GamingWiki.Models;
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
                        Replier = r.Replier.UserName,
                        ReplierId = r.ReplierId
                    }).ToList();

        public int Add(string content, int commentId, string replierId)
        {
            var reply = new Reply
            {
                Content = content,
                CommentId = commentId,
                ReplierId = replierId,
                AddedOn = DateTime.UtcNow
            };

            this.dbContext.Replies.Add(reply);
            this.dbContext.SaveChanges();

            var articleId = this.dbContext.Comments
                .Where(c => c.Id == commentId)
                .Select(c => c.ArticleId)
                .FirstOrDefault();

            return articleId;
        }

        public int Delete(int replyId)
        {
            var reply = this.dbContext.Replies
                .FirstOrDefault(r => r.Id == replyId);

            if (reply == null)
            {
                return 0;
            }

            var articleId = this.dbContext.Replies
                .Where(r => r.Id == replyId)
                .Select(r => r.Comment.ArticleId)
                .FirstOrDefault();

            this.dbContext.Replies.Remove(reply);
            this.dbContext.SaveChanges();

            return articleId;
        }

        public bool ReplyExists(int replyId)
            => this.dbContext.Replies.Any(r => r.Id == replyId);

        public string GetReplyAuthorId(int replyId)
            => this.dbContext.Replies
                .First(r => r.Id == replyId).ReplierId;
    }
}
