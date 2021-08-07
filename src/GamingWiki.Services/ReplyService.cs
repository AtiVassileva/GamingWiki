using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Replies;

namespace GamingWiki.Services
{
    public class ReplyService : IReplyService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfigurationProvider configuration;

        public ReplyService(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.configuration = mapper.ConfigurationProvider;
        }

        public IEnumerable<ReplyServiceModel> AllByComment(int commentId) 
            => this.dbContext.Replies
                .Where(r => r.CommentId == commentId)
                .ProjectTo<ReplyServiceModel>(this.configuration)
                .ToList();

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

            var articleId = this.GetArticleIdByComment(commentId);

            return articleId;
        }

        public bool Delete(int replyId)
        {
            if (!this.ReplyExists(replyId))
            {
                return false;
            }

            var reply = this.FindReply(replyId);
            
            this.dbContext.Replies.Remove(reply);
            this.dbContext.SaveChanges();

            return true;
        }

        public bool ReplyExists(int replyId)
            => this.dbContext.Replies.Any(r => r.Id == replyId);

        public string GetReplyAuthorId(int replyId)
            => this.dbContext.Replies
                .First(r => r.Id == replyId).ReplierId;
         
        public int GetArticleIdByReply(int replyId)
        => this.dbContext.Replies
            .Where(r => r.Id == replyId)
            .Select(r => r.Comment.ArticleId)
            .First();
        
        private int GetArticleIdByComment(int commentId)
        => this.dbContext.Comments
            .Where(c => c.Id == commentId)
            .Select(c => c.ArticleId)
            .FirstOrDefault();

        private Reply FindReply(int replyId)
            => this.dbContext.Replies.First(r => r.Id == replyId);
    }
}
