using Microsoft.AspNetCore.Identity;

namespace GamingWiki.Tests.Data
{
    public static class Users
    {
        private const string DefaultTestUserName = "TestUser";
        public static IdentityUser TestUser 
            => new(DefaultTestUserName);
    }
}
