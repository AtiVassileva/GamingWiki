using Microsoft.AspNetCore.Identity;

namespace GamingWiki.Models
{
    public class User : IdentityUser
    {
        public string Username { get; set; }

        public string Password { get; set; }

    }
}
