using GamingWiki.Web.Models;

namespace GamingWiki.Web.Common
{
    public class WebConstants
    {
        public const string AdministratorRoleName = "Administrator";

        public const int PasswordRequiredLength = 8;

        public static ErrorViewModel CreateError(string message)
            => new() { Message = message };
    }
}
