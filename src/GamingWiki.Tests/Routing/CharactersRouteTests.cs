using GamingWiki.Services.Models.Characters;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models.Characters;
using static GamingWiki.Tests.Data.Characters;
using static GamingWiki.Tests.Data.Classes;
using static GamingWiki.Tests.Common.TestConstants;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace GamingWiki.Tests.Routing
{
    public class CharactersRouteTests
    {
        [Fact]
        public void AllShouldBeMappedWithNoPageParameter()
            => MyRouting
                .Configuration()
                .ShouldMap("/Characters/All")
                .To<CharactersController>(c =>
                    c.All(With.No<int>()));

        [Fact]
        public void AllShouldBeMappedWithPageParameter()
            => MyRouting
                .Configuration()
                .ShouldMap("/Characters/All?pageIndex=1")
                .To<CharactersController>(c =>
                    c.All(1));

        [Fact]
        public void GetCreateShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Characters/Create")
                .To<CharactersController>(c => c.Create());


        [Fact]
        public void PostCreateShouldBeMapped()
        => MyRouting
            .Configuration()
        .ShouldMap(request => request
            .WithMethod(HttpMethod.Post)
        .WithLocation("/Characters/Create"))
            .To<CharactersController>(c =>
        c.Create(With.Any<CharacterFormModel>()));

        [Fact]
        public void DetailsShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Characters/Details?characterId=1")
                .To<CharactersController>(c =>
                    c.Details(1));

        [Fact]
        public void GetEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Characters/Edit?characterId=1")
                .To<CharactersController>(c =>
                    c.Edit(1));

        [Fact]
        public void PostEditShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"/Characters/Edit?characterId={TestCharacter.Id}"))
                .To<CharactersController>(c =>
                    c.Edit(With.Any<CharacterServiceEditModel>(), 
                        TestCharacter.Id));

        [Fact]
        public void DeleteShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Characters/Delete?characterId=1")
                .To<CharactersController>(c =>
                    c.Delete(1));

        [Fact]
        public void SearchShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Characters/Search?parameter=a")
                .To<CharactersController>(c =>
                    c.Search("a", With.No<int>()));

        [Fact]
        public void SearchShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap("/Characters/Search?parameter=a&pageIndex=1")
            .To<CharactersController>(c =>
                c.Search("a", 1));

        [Fact]
        public void FilterShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Characters/Filter?parameter=1")
                .To<CharactersController>(c =>
                    c.Filter(1, With.No<int>()));

        [Fact]
        public void FilterShouldBeMappedWithPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Characters/Filter?parameter=1&pageIndex=1")
                .To<CharactersController>(c =>
                    c.Filter(1, 1));


        [Fact]
        public void MineShouldBeMappedWithoutPageIndex()
            => MyRouting
                .Configuration()
                .ShouldMap("/Characters/Mine")
                .To<CharactersController>(c =>
                    c.Mine(With.No<int>()));

        [Fact]
        public void MineShouldBeMappedWithPageIndex()
        => MyRouting
            .Configuration()
            .ShouldMap("/Characters/Mine?pageIndex=1")
            .To<CharactersController>(c =>
                c.Mine(1));
    }
}
