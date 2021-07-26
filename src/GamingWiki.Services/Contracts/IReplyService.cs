using System.Collections.Generic;
using GamingWiki.Services.Models.Replies;

namespace GamingWiki.Services.Contracts
{
    public interface IReplyService
    {
        IEnumerable<ReplyServiceModel> AllByComment(int commentId);
    }
}
