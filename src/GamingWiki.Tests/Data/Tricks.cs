using System.Collections.Generic;
using System.Linq;
using GamingWiki.Models;
using GamingWiki.Services.Models.Tricks;
using GamingWiki.Web.Models.Tricks;
using static GamingWiki.Tests.Common.TestConstants;
using static GamingWiki.Tests.Data.Games;
using static GamingWiki.Tests.Data.Users;

namespace GamingWiki.Tests.Data
{
    public static class Tricks
    {
        public static IEnumerable<Trick> FiveTricks
            => Enumerable.Range(0, 5).Select(_ => new Trick
            {
                GameId = TestGame.Id
            });

        public static TrickFormModel TestTrickValidFormModel
            => new()
            {
                Id = DefaultId,
                Heading = DefaultHeading,
                Content = DefaultContent,
                PictureUrl = DefaultUrl,
                GameId = TestGame.Id
            };

        public static Trick TestTrick => new()
        {
            Id = DefaultId,
            Author = TestUser,
            Game = TestGame
        };

        public static TrickServiceEditModel TestTrickValidEditModel
            => new()
            {
                Heading = DefaultHeading,
                Content = DefaultContent,
                PictureUrl = DefaultUrl
            };

        public static TrickServiceEditModel TestTrickInvalidEditModel
            => new()
            {
                Heading = "a",
                Content = "b",
                PictureUrl = "c"
            };
    }
}
