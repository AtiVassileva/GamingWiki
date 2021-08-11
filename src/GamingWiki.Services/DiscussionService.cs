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

        public int Create(string creatorId, string description)
        {
            var discussion = new Discussion
            {
                CreatorId = creatorId,
                Description = description
            };

            this.dbContext.Discussions.Add(discussion);
            this.dbContext.SaveChanges();

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

        private Discussion FindDiscussion(int discussionId)
            => this.dbContext.Discussions
                .Include(d => d.Creator)
                .First(d => d.Id == discussionId);
    }
}
