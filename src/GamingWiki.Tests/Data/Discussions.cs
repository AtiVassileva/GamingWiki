using System;
using System.Collections.Generic;
using System.Linq;
using GamingWiki.Models;
using GamingWiki.Services.Models.Discussions;
using GamingWiki.Web.Models.Discussions;
using static GamingWiki.Tests.Common.TestConstants;
using static GamingWiki.Models.Common.DataConstants;
using static GamingWiki.Tests.Data.Users;

namespace GamingWiki.Tests.Data
{
    public static class Discussions
    {
        public static IEnumerable<Discussion> FiveDiscussions
            => Enumerable.Range(0, 5).Select(_ => new Discussion());

        public static DiscussionFormModel TestValidDiscussionFormModel
            => new()
            {
                Name = DefaultName,
                Description = DefaultDescription,
                PictureUrl = DefaultPictureUrl,
                MembersLimit = GenerateRandomLimit
            };

        public static DiscussionFormModel TestInvalidDiscussionFormModel
            => new()
            {
                Name = "a",
                Description = "b",
                PictureUrl = "c",
                MembersLimit = GenerateRandomLimit
            };

        public static Discussion TestDiscussion
            => new()
            {
                Id = DefaultId,
                Name = DefaultName,
                Description = DefaultDescription,
                PictureUrl = DefaultPictureUrl,
                MembersLimit = DefaultMemberLimit,
                Creator = TestUser
            };

        public static DiscussionServiceEditModel TestValidDiscussionEditModel
            => new()
            {
                Id = DefaultId,
                Name = DefaultName,
                Description = DefaultDescription,
                PictureUrl = DefaultPictureUrl,
                MembersLimit = GenerateRandomLimit
            };

        public static DiscussionServiceEditModel TestInvalidDiscussionEditModel
            => new()
            {
                Id = DefaultId,
                Name = "a",
                Description = "b",
                PictureUrl = "c",
                MembersLimit = GenerateRandomLimit
            };

        public static IEnumerable<UserDiscussion> FullTestUserDiscussion
            => Enumerable.Range(0, TestDiscussion.MembersLimit)
                .Select(_ => new UserDiscussion
                {
                    UserId = Guid.NewGuid().ToString(),
                    DiscussionId = TestDiscussion.Id
                });

        public static UserDiscussion TestUserDiscussionWithTestUser
            => new()
            {
                UserId = MyTested.AspNetCore.Mvc.TestUser.Identifier,
                DiscussionId = TestDiscussion.Id
            };

        private static int GenerateRandomLimit
            => new Random().Next(MinDiscussionMembers, MaxDiscussionMembers);
    }
}
