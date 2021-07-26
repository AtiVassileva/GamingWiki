using System.Collections.Generic;
using GamingWiki.Services.Models.Replies;

namespace GamingWiki.Services.Contracts
{
    public interface IReplyService
    {
        IEnumerable<ReplyServiceModel> AllByComment(int commentId);

        int Add(string content, int commentId, string replierId);

        int Delete(int replyId);
    }
}
