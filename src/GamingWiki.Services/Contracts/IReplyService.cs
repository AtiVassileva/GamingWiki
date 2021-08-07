using System.Collections.Generic;
using GamingWiki.Services.Models.Replies;

namespace GamingWiki.Services.Contracts
{
    public interface IReplyService
    {
        IEnumerable<ReplyServiceModel> AllByComment(int commentId);

        int Add(string content, int commentId, string replierId);

        bool Delete(int replyId);

        bool ReplyExists(int replyId);

        int GetArticleIdByReply(int replyId);

        string GetReplyAuthorId(int replyId);
    }
}
