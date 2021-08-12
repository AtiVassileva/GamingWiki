using System.Linq;
using GamingWiki.Services.Models.Discussions;

namespace GamingWiki.Services.Contracts
{
    public interface IDiscussionService
    {
        IQueryable<DiscussionAllServiceModel> All();

        int Create(string creatorId, string name, string description);

        DiscussionServiceDetailsModel Details(int discussionId);

        bool DiscussionExists(int discussionId);

        string GetCreatorId(int discussionId);

        bool Edit(int discussionId, DiscussionServiceEditModel model);

        bool Delete(int discussionId);

        IQueryable<DiscussionAllServiceModel> Search(string searchCriteria);
    }
}
