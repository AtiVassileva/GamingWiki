using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Discussions;
using GamingWiki.Services.Models.Messages;
using Microsoft.EntityFrameworkCore;

namespace GamingWiki.Services
{
    public class DiscussionService : IDiscussionService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IConfigurationProvider configuration;

        public DiscussionService(ApplicationDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.configuration = mapper.ConfigurationProvider;
        }

        public IQueryable<DiscussionAllServiceModel> All()
            => this.dbContext.Discussions
                .ProjectTo<DiscussionAllServiceModel>(this.configuration)
                .OrderByDescending(d => d.Id);

        public int Create(string creatorId, string name, string description)
        {
            var discussion = new Discussion
            {
                Name = name,
                CreatorId = creatorId,
                Description = description
            };

            this.dbContext.Discussions.Add(discussion);
            this.dbContext.SaveChanges();

            JoinUserToDiscussion(discussion.Id, creatorId);
            return discussion.Id;
        }

        public DiscussionServiceDetailsModel Details(int discussionId)
        {
            
            var discussion = this.FindDiscussion(discussionId);

            var detailsModel = this.mapper
                .Map<DiscussionServiceDetailsModel>(discussion);

            return detailsModel;
        }

        public bool DiscussionExists(int discussionId)
            => this.dbContext.Discussions
                .Any(d => d.Id == discussionId);

        public string GetCreatorId(int discussionId)
            => this.dbContext.Discussions
                .Where(d => d.Id == discussionId)
                .Select(d => d.CreatorId)
                .FirstOrDefault();

        public bool Edit(int discussionId, DiscussionServiceEditModel model)
        {
            if (!this.DiscussionExists(discussionId))
            {
                return false;
            }

            var discussion = this.FindDiscussion(discussionId);

            discussion.Name = model.Name;
            discussion.Description = model.Description;

            this.dbContext.SaveChanges();

            return true;
        }

        public bool Delete(int discussionId)
        {
            if (!this.DiscussionExists(discussionId))
            {
                return false;
            }

            var discussion = this.FindDiscussion(discussionId);

            this.dbContext.Discussions.Remove(discussion);
            this.dbContext.SaveChanges();

            return true;
        }

        public void JoinUserToDiscussion(int discussionId, string userId)
        {
            var userDiscussion = new UserDiscussion
            {
                UserId = userId,
                DiscussionId = discussionId
            };

            this.dbContext.UserDiscussion.Add(userDiscussion);
            this.dbContext.SaveChanges();
        }

        public bool UserParticipatesInDiscussion(int discussionId, string userId)
            => this.dbContext.UserDiscussion
                .Any(ud => ud.UserId == userId 
                           && ud.DiscussionId == discussionId);

        public IQueryable<DiscussionAllServiceModel> Search(string searchCriteria)
            => this.dbContext.Discussions
                .Where(d => d.Name.ToLower()
                    .Contains(searchCriteria.ToLower()))
                .ProjectTo<DiscussionAllServiceModel>(this.configuration);

        public IEnumerable<MessageServiceModel> GetMessagesForDiscussion(int discussionId)
            => this.dbContext.Messages
                .Where(m => m.DiscussionId == discussionId)
                .OrderBy(m => m.Id)
                .ProjectTo<MessageServiceModel>(this.configuration)
                .ToList();

        public int AddMessage(int discussionId, string content, string senderId)
        {
            var message = new Message
            {
                DiscussionId = discussionId,
                Content = content,
                SenderId = senderId,
                SentOn = DateTime.UtcNow
            };

            this.dbContext.Messages.Add(message);
            this.dbContext.SaveChanges();

            return message.Id;
        }

        private Discussion FindDiscussion(int discussionId)
            => this.dbContext.Discussions
                .Include(d => d.Creator)
                .First(d => d.Id == discussionId);
    }
}
