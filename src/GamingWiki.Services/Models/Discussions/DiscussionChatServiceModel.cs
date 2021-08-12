using System.Collections.Generic;
using GamingWiki.Services.Models.Messages;

namespace GamingWiki.Services.Models.Discussions
{
    public class DiscussionChatServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<MessageServiceModel> Messages { get; set; }
    }
}
