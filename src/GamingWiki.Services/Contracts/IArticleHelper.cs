using Microsoft.AspNetCore.Identity;

namespace GamingWiki.Services.Contracts
{
    public interface IArticleHelper
    {
        IdentityUser GetUser(string userId);
    }
}
