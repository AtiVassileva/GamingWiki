using System.Linq;
using GamingWiki.Services.Models.Discussions;

namespace GamingWiki.Services.Contracts
{
    public interface IDiscussionService
    {
        IQueryable<DiscussionAllServiceModel> All();

        int Create(string creatorId, string description);

        DiscussionServiceDetailsModel Details(int discussionId);

        bool DiscussionExists(int discussionId);
    }
}
