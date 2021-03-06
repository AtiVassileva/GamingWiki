namespace GamingWiki.Services.Models.Discussions
{
    public class DiscussionServiceDetailsModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string PictureUrl { get; set; }

        public string CreatorId { get; set; }

        public string CreatorName { get; set; }

        public int CurrentMembersCount { get; set; }

        public int MembersLimit { get; set; }
    }
}
