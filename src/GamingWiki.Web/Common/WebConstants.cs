using GamingWiki.Web.Models;

namespace GamingWiki.Web.Common
{
    public class WebConstants
    {
        public const int PasswordRequiredLength = 8;

        public const string GlobalMessageKey = "GlobalMessage";

        public const string ColorKey = "Color";

        public static ErrorViewModel CreateError(string message)
            => new() { Message = message };
    }
}
