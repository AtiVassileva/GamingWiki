using System.Collections.Generic;
using System.Linq;
using GamingWiki.Services.Models.Discussions;
using GamingWiki.Services.Models.Messages;

namespace GamingWiki.Services.Contracts
{
    public interface IDiscussionService
    {
        IQueryable<DiscussionAllServiceModel> All();

        int Create(string creatorId, string name, string description,
            string pictureUrl, int membersLimit);

        DiscussionServiceDetailsModel Details(int discussionId);

        bool DiscussionExists(int discussionId);

        string GetCreatorId(int discussionId);

        bool Edit(int discussionId, DiscussionServiceEditModel model);

        bool Delete(int discussionId);

        int AddMessage(int discussionId, string content, string senderId);

        void JoinUserToDiscussion(int discussionId, string userId);

        void RemoveUserFromDiscussion(int discussionId, string userId);

        bool UserParticipatesInDiscussion(int discussionId, string userId);

        bool DiscussionFull(int discussionId);

        int GetMembersCount(int discussionId);

        IQueryable<DiscussionAllServiceModel> Search(string searchCriteria);
        
        IQueryable<DiscussionAllServiceModel> GetDiscussionsByUser(string userId);

        IEnumerable<MessageServiceModel> GetMessagesForDiscussion
            (int discussionId);
    }
}
