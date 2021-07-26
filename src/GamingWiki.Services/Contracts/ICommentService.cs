using System.Collections.Generic;
using GamingWiki.Services.Models.Comments;

namespace GamingWiki.Services.Contracts
{
    public interface ICommentService
    {
        IEnumerable<CommentServiceModel>AllByArticle(int articleId);
    }
}
