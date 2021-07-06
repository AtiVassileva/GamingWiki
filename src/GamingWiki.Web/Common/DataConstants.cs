namespace GamingWiki.Web.Common
{
    public class DataConstants
    {
        public const int DefaultMinLength = 3;
        public const int DescriptionMinLength = 10;
        public const int DefaultMaxLength = 100;

        //Game
        public const string ValidPlaceNameRegex = @"[A-Za-z\s\.0-9]+";

        //Article
        public const int ContentMinLength = 20;
    }
}
