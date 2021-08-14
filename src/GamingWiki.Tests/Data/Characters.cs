﻿using System.Collections.Generic;
using System.Linq;
using GamingWiki.Models;
using GamingWiki.Services.Models.Characters;
using GamingWiki.Web.Models.Characters;
using static GamingWiki.Tests.Data.Classes;
using static GamingWiki.Tests.Data.Games;
using static GamingWiki.Tests.Data.Users;
using static GamingWiki.Tests.Common.TestConstants;

namespace GamingWiki.Tests.Data
{
    public static class Characters
    {
        public static IEnumerable<Character> FiveCharacters
            => Enumerable.Range(0, 5).Select(_ => new Character());

        public static Character TestCharacter =>
            new ()
            {
                Id = DefaultId,
                Name = DefaultName,
                Description = DefaultDescription,
                ContributorId = TestUser.Id,
                ClassId = TestCharacterClass.Id,
                Class = TestCharacterClass,
                GameId = TestGame.Id,
                Game = TestGame,
                IsApproved = true
            };

        public static CharacterFormModel TestCharacterFormModel
            => new ()
            {
                Name = DefaultName,
                Description = DefaultDescription,
                PictureUrl = DefaultPictureUrl,
                ClassId = TestCharacterClass.Id,
                GameId = TestGame.Id
            };

        public static CharacterServiceEditModel TestValidCharacterEditModel
            => new()
            {
                Id = TestCharacter.Id,
                Name = DefaultName,
                Description = DefaultDescription,
                PictureUrl = DefaultPictureUrl,
                ClassId = TestCharacterClass.Id,
                ClassName = TestCharacterClass.Name,
                IsApproved = true
            };

        public static CharacterServiceEditModel TestInvalidCharacterEditModel
            => new()
            {
                Id = TestCharacter.Id,
                Name = "a",
                Description = "b",
                ClassId = 6,
                PictureUrl = DefaultPictureUrl,
                ClassName = "c",
                ContributorId = TestUser.Id,
                IsApproved = false
            };
    }
}
