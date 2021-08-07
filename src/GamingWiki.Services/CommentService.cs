using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Comments;
using Microsoft.EntityFrameworkCore;

namespace GamingWiki.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IReplyService replyService;
        private readonly IMapper mapper;

        public CommentService(ApplicationDbContext dbContext, IReplyService replyService, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.replyService = replyService;
            this.mapper = mapper;
        }

        public IEnumerable<CommentServiceModel> AllByArticle(int articleId)
        {
            var comments = this.dbContext.Comments
                .Where(c => c.ArticleId == articleId)
                .Include(c => c.Commenter);

            foreach (var comment in comments)
            {
                var currentComment = this.mapper
                    .Map<CommentServiceModel>(comment);

                currentComment.Replies = 
                    this.replyService.AllByComment(currentComment.Id);

                yield return currentComment;
            }
        }

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

        public bool Delete(int commentId)
        {
            if (!this.CommentExists(commentId))
            {
                return false;
            }

            var comment = this.FindComment(commentId);

            this.dbContext.Comments.Remove(comment);
            this.dbContext.SaveChanges();

            return true;
        }

        public bool CommentExists(int commentId)
            => this.dbContext.Comments.Any(c => c.Id == commentId);

        public string GetCommentAuthorId(int commentId)
            => this.dbContext.Comments
                .First(c => c.Id == commentId).CommenterId;

        public int GetArticleId(int commentId)
            => this.dbContext.Comments
                .Where(c => c.Id == commentId)
                .Select(c => c.ArticleId)
                .First();

        public Comment FindComment(int commentId)
            => this.dbContext.Comments.First(c => c.Id == commentId);
    }
}
