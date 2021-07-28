using System.Collections.Generic;
using GamingWiki.Services.Models.Comments;

namespace GamingWiki.Services.Contracts
{
    public interface ICommentService
    {
        IEnumerable<CommentServiceModel>AllByArticle(int articleId);

        int Add(int articleId, string content, string commenterId);

        int Delete(int commentId);

        bool CommentExists(int commentId);

        string GetCommentAuthorId(int commentId);
    }
}
