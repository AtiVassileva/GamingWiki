using Microsoft.AspNetCore.Identity;
using static GamingWiki.Tests.Common.TestConstants;

namespace GamingWiki.Tests.Data
{
    public static class Users
    { 
        public static IdentityUser TestUser 
            => new(DefaultTestUserName);
    }
}
