using System;
using System.Collections.Generic;
using System.Linq;
using GamingWiki.Models;
using GamingWiki.Services.Models.Games;
using GamingWiki.Web.Models.Games;
using static GamingWiki.Tests.Common.TestConstants;
using static GamingWiki.Tests.Data.Areas;
using static GamingWiki.Tests.Data.Genres;

namespace GamingWiki.Tests.Data
{
    public static class Games
    {
        public static Game TestGame => new () 
            { 
                Id = DefaultId, 
                Name = DefaultName,
                Area = TestArea,
                Genre = TestGenre
            };

        public static IEnumerable<Game> FiveGames
            => Enumerable.Range(0, 5).Select(_ => new Game());

        public static GameFormModel TestValidGameFormModel
            => new()
            {
                Name = DefaultName,
                Description = DefaultDescription,
                AreaId = TestArea.Id,
                GenreId = TestGenre.Id,
                PictureUrl = DefaultUrl,
                TrailerUrl = DefaultUrl,
                CreatorsNames = new Guid().ToString(),
                SupportedPlatforms = GenerateRandomPlatforms
            };
        
        public static GameFormModel TestInvalidGameFormModel
            => new()
            {
                Name = "a",
                Description = "b",
                AreaId = TestArea.Id,
                GenreId = TestGenre.Id,
                PictureUrl = "c",
                TrailerUrl = "c",
                CreatorsNames = new Guid().ToString(),
                SupportedPlatforms = GenerateRandomPlatforms
            };

        public static GameServiceEditModel TestValidGameEditModel
            => new()
            {
                Name = DefaultName,
                Description = DefaultDescription,
                AreaId = TestArea.Id,
                PictureUrl = DefaultUrl,
                TrailerUrl = DefaultUrl,
                SupportedPlatforms = GenerateRandomPlatforms
            };
        
        public static GameServiceEditModel TestValidGameEditModelWithInvalidAreaId
            => new()
            {
                Name = DefaultName,
                Description = DefaultDescription,
                AreaId = -1,
                PictureUrl = DefaultUrl,
                TrailerUrl = DefaultUrl,
                SupportedPlatforms = GenerateRandomPlatforms
            };

        public static GameServiceEditModel TestInvalidGameEditModel
            => new()
            {
                Name = "a",
                Description = "b",
                AreaId = TestArea.Id,
                PictureUrl = "c",
                TrailerUrl = "c",
                SupportedPlatforms = GenerateRandomPlatforms
            };

        private static IEnumerable<int> GenerateRandomPlatforms
            => new List<int>(new Random().Next(0, 5));
    }
}
