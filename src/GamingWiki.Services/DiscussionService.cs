using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Discussions;
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

            AddDiscussionToUser(discussion.Id, creatorId);
            return discussion.Id;
        }

        private void AddDiscussionToUser(int discussionId, string creatorId)
        {
            var userDiscussion = new UserDiscussion
            {
                UserId = creatorId,
                DiscussionId = discussionId
            };

            this.dbContext.UserDiscussion.Add(userDiscussion);
            this.dbContext.SaveChanges();
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

        public IQueryable<DiscussionAllServiceModel> Search(string searchCriteria)
            => this.dbContext.Discussions
                .Where(d => d.Name.ToLower()
                    .Contains(searchCriteria.ToLower()))
                .ProjectTo<DiscussionAllServiceModel>(this.configuration);

        private Discussion FindDiscussion(int discussionId)
            => this.dbContext.Discussions
                .Include(d => d.Creator)
                .First(d => d.Id == discussionId);
    }
}
