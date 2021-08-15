using GamingWiki.Models;
using static GamingWiki.Tests.Common.TestConstants;
using static GamingWiki.Tests.Data.Discussions;

namespace GamingWiki.Tests.Data
{
    public static class Messages
    {
        public static Message TestMessage => new()
        {
            Id = DefaultId,
            DiscussionId = TestDiscussion.Id
        };
    }
}
