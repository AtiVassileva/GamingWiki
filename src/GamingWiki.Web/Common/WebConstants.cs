using GamingWiki.Web.Models;

namespace GamingWiki.Web.Common
{
    public class WebConstants
    {
        public const int PasswordRequiredLength = 8;

        public const string GlobalMessageKey = "GlobalMessage";

        public const string ColorKey = "Color";

        public const string DangerAlertColor = "danger";

        public static ErrorViewModel CreateError(string message)
            => new() { Message = message };

        public class Cache
        {
            public const int CacheExpiringTime = 20;

            public const string LatestArticlesCacheKey
                = nameof(LatestArticlesCacheKey);

            public const string LatestGamesCacheKey 
                = nameof(LatestGamesCacheKey);

            public const string LatestTricksCacheKey
                = nameof(LatestTricksCacheKey);
        }
    }
}
