using System;
using System.Collections.Generic;
using System.Linq;
using GamingWiki.Data;
using GamingWiki.Models;
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
                        CommenterId = c.CommenterId,
                        AddedOn = c.AddedOn.ToString(DateFormat),
                        Replies = this.replyService.AllByComment(c.Id)
                    }).ToList();

        public int Add(int articleId, string content, string commenterId)
        {
            var comment = new Comment
            {
                Content = content,
                ArticleId = articleId,
                CommenterId = commenterId,
                AddedOn = DateTime.UtcNow
            };

            this.dbContext.Comments.Add(comment);
            this.dbContext.SaveChanges();

            return comment.Id;
        }

        public int Delete(int commentId)
        {
            var comment = this.dbContext.Comments
                .FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
            {
                return 0;
            }

            var articleId = this.dbContext.Comments
                .Where(c => c.Id == commentId)
                .Select(c => c.ArticleId)
                .FirstOrDefault();

            this.dbContext.Comments.Remove(comment);
            this.dbContext.SaveChanges();

            return articleId;
        }
    }
}
