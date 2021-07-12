﻿namespace GamingWiki.Models.Common
{
    public class DataConstants
    {
        public const int DefaultMinLength = 3;
        public const int DescriptionMinLength = 10;
        public const int DefaultMaxLength = 100;

        public const string ValidPlaceNameRegex = @"[A-Za-z\s\.0-9]+";

        public const int ContentMinLength = 20;
        public const int HeadingMaxLength = 30;
        public const int CategoryNameMaxLength = 25;

        public const int CharacterNameMaxLength = 30;
        public const int CharacterClassMaxLength = 25;

        public const int CommentContentMaxLength = 2000;
        public const int CreatorNameMaxLength = 80;

        public const int GameNameMaxLength = 30;
        public const int AreaNameMaxLength = 25;
        public const int GenreNameMaxLength = 40;
    }
}