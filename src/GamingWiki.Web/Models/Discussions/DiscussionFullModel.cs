using GamingWiki.Services.Models.Discussions;

namespace GamingWiki.Web.Models.Discussions
{
    public class DiscussionFullModel : BaseFullModel
    {
        public PaginatedList<DiscussionAllServiceModel> Discussions { get; set; }
    }
}
