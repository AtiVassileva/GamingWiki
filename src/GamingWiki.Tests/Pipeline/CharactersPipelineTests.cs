using System.Collections.Generic;
using GamingWiki.Services.Models.Characters;
using GamingWiki.Services.Models.Classes;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models;
using GamingWiki.Web.Models.Characters;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Common.ExceptionMessages;
using MyTested.AspNetCore.Mvc;
using Shouldly;
using Xunit;
using static GamingWiki.Tests.Data.Characters;

namespace GamingWiki.Tests.Pipeline
{
    public class CharactersPipelineTests
    {
        [Fact]
        public void AllWithoutPageShouldReturnCorrectViewWithCorrectModel()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Characters/All")
                .WithUser()
                .WithAntiForgeryToken())
            .To<CharactersController>(c => c.All(With.No<int>()))
            .Which(controller => controller
                .WithData(FiveCharacters))
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<CharacterFullModel>(m =>
                {
                    m.Characters.ShouldBeOfType(typeof(PaginatedList<CharacterAllServiceModel>));
                    m.Classes.ShouldBeOfType(typeof(List<ClassSimpleServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                })
                .Passing(characterListing =>
                {
                    characterListing.Characters.Count.ShouldBe(3);
                    characterListing.Characters.PageIndex.ShouldBe(0);
                    characterListing.Characters.TotalPages.ShouldBe(2);
                }));

        [Theory]
        [InlineData(1, 3, 2)]
        [InlineData(2, 2, 2)]
        public void AllWithPageShouldReturnCorrectViewWithCorrectModel(int pageIndex, int expectedCountOnPage, int totalPages)
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Characters/All?pageIndex={pageIndex}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<CharactersController>(c => c.All(pageIndex))
                .Which(controller => controller
                    .WithData(FiveCharacters))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CharacterFullModel>(m =>
                    {
                        m.Characters.ShouldBeOfType(typeof(PaginatedList<CharacterAllServiceModel>));
                        m.Classes.ShouldBeOfType(typeof(List<ClassSimpleServiceModel>));
                        m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                    })
                    .Passing(characterListing =>
                    {
                        characterListing.Characters.Count.ShouldBe(expectedCountOnPage);
                        characterListing.Characters.PageIndex.ShouldBe(pageIndex);
                        characterListing.Characters.TotalPages.ShouldBe(totalPages);
                    }));

        [Fact]
        public void GetCreateShouldBeMappedAndReturnCorrectView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation("/Characters/Create")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<CharactersController>(c => c.Create())
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CharacterFormModel>());

        [Fact]
        public void PostCreateShouldBeMappedAndHaveInvalidModelStateAndReturnView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation("/Characters/Create")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<CharactersController>(c => c.Create(new CharacterFormModel()))
                .Which()
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CharacterFormModel>());

        [Fact]
        public void DetailsShouldBeMappedAndReturnCorrectViewWithValidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Characters/Details?characterId={TestCharacter.Id}")
                .WithUser()
                .WithAntiForgeryToken())
            .To<CharactersController>(c => c.Details(TestCharacter.Id))
            .Which(controller => controller
                .WithData(TestCharacter))
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<CharacterServiceDetailsModel>());

        [Fact]
        public void DetailsShouldBeMappedAndReturnErrorViewWithInvalidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Characters/Details?characterId={TestCharacter.Id}")
                .WithUser()
                .WithAntiForgeryToken())
            .To<CharactersController>(c => c.Details(TestCharacter.Id))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ErrorViewModel>(m =>
                    m.Message == NonExistingCharacterExceptionMessage));

        [Fact]
        public void GetEditShouldBeMappedAndReturnCorrectViewWithValidId()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Characters/Edit?characterId={TestCharacter.Id}")
                    .WithUser(user => user.InRole(AdministratorRoleName))
                    .WithAntiForgeryToken())
                .To<CharactersController>(c => c.Edit(TestCharacter.Id))
                .Which(controller => controller
                    .WithData(TestCharacter))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CharacterServiceEditModel>());

        [Fact]
        public void GetEditShouldBeMappedAndReturnUnauthorizedForUnauthorizedUsers()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Characters/Edit?characterId={TestCharacter.Id}")
                    .WithUser(TestUser.Identifier)
                    .WithAntiForgeryToken())
                .To<CharactersController>(c => c.Edit(TestCharacter.Id))
                .Which(controller => controller
                    .WithData(TestCharacter))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void PostEditShouldBeMappedAndHaveInvalidModelStateAndReturnView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithMethod(HttpMethod.Post)
                    .WithLocation($"/Characters/Edit?characterId={TestCharacter.Id}")
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<CharactersController>(c => c.Edit(new CharacterServiceEditModel(), TestCharacter.Id))
                .Which(controller => controller
                    .WithData(TestCharacter))
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CharacterServiceEditModel>());

        [Fact]
        public void GetEditShouldBeMappedAndReturnErrorViewWithInvalidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Characters/Edit?characterId={TestCharacter.Id}")
                .WithUser(TestUser.Identifier)
                .WithAntiForgeryToken())
            .To<CharactersController>(c => c.Edit(TestCharacter.Id))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ErrorViewModel>(m =>
                    m.Message == NonExistingCharacterExceptionMessage));

        [Fact]
        public void DeleteShouldBeMappedAndRedirectUponSuccessfulAction()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithLocation($"/Characters/Delete?characterId={TestCharacter.Id}")
                    .WithUser(user => user.InRole(AdministratorRoleName))
                    .WithAntiForgeryToken())
                .To<CharactersController>(c => c.Delete(TestCharacter.Id))
                .Which(controller => controller
                    .WithData(TestCharacter))
                .ShouldHave()
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<CharactersController>(c => c
                        .All(With.Any<int>())));

        [Fact]
        public void DeleteShouldBeMappedAndReturnUnauthorizedForUnauthorizedUsers()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Characters/Delete?characterId={TestCharacter.Id}")
                .WithUser(TestUser.Identifier)
                .WithAntiForgeryToken())
            .To<CharactersController>(c => c.Delete(TestCharacter.Id))
            .Which(controller => controller
                .WithData(TestCharacter))
            .ShouldReturn()
            .Unauthorized();

        [Fact]
        public void DeleteShouldBeMappedAndReturnErrorViewWithInvalidId()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation($"/Characters/Delete?characterId={TestCharacter.Id}")
                .WithUser(user => user.InRole(AdministratorRoleName))
                .WithAntiForgeryToken())
            .To<CharactersController>(c =>
                c.Delete(TestCharacter.Id))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<ErrorViewModel>(m =>
                    m.Message == NonExistingCharacterExceptionMessage));

        [Fact]
        public void SearchShouldBeMappedAndReturnCorrectViewWithPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Characters/Search?parameter=a&pageIndex=1")
                .WithUser()
                .WithAntiForgeryToken())
            .To<CharactersController>(c =>
                c.Search("a", 1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<CharacterFullModel>(m =>
                {
                    m.Characters.ShouldBeOfType(typeof(PaginatedList<CharacterAllServiceModel>));
                    m.Classes.ShouldBeOfType(typeof(List<ClassSimpleServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));

        [Fact]
        public void SearchShouldBeMappedAndReturnCorrectViewWithoutPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
            .WithLocation("/Characters/Search?parameter=a")
            .WithUser()
            .WithAntiForgeryToken())
            .To<CharactersController>(c =>
        c.Search("a", With.No<int>()))
            .Which()
            .ShouldReturn()
            .View(view => view
            .WithModelOfType<CharacterFullModel>(m =>
            {
                m.Characters.ShouldBeOfType(typeof(PaginatedList<CharacterAllServiceModel>));
                m.Classes.ShouldBeOfType(typeof(List<ClassSimpleServiceModel>));
                m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
            }));

        [Fact]
        public void FilterShouldBeMappedAndReturnCorrectViewWithoutPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Characters/Filter?parameter=1")
                .WithUser()
                .WithAntiForgeryToken())
            .To<CharactersController>(c =>
                c.Filter(1, With.No<int>()))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<CharacterFullModel>(m =>
                {
                    m.Characters.ShouldBeOfType(typeof(PaginatedList<CharacterAllServiceModel>));
                    m.Classes.ShouldBeOfType(typeof(List<ClassSimpleServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));

        [Fact]
        public void FilterShouldBeMappedAndReturnCorrectViewWithPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Characters/Filter?parameter=1&pageIndex=1")
                .WithUser()
                .WithAntiForgeryToken())
            .To<CharactersController>(c =>
                c.Filter(1, 1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<CharacterFullModel>(m =>
                {
                    m.Characters.ShouldBeOfType(typeof(PaginatedList<CharacterAllServiceModel>));
                    m.Classes.ShouldBeOfType(typeof(List<ClassSimpleServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));

        [Fact]
        public void MineShouldBeMappedAndReturnCorrectViewWithPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Characters/Mine?pageIndex=1")
                .WithUser()
                .WithAntiForgeryToken())
            .To<CharactersController>(c =>
                c.Mine(1))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<CharacterFullModel>(m =>
                {
                    m.Characters.ShouldBeOfType(typeof(PaginatedList<CharacterAllServiceModel>));
                    m.Classes.ShouldBeOfType(typeof(List<ClassSimpleServiceModel>));
                    m.Tokens.ShouldBeOfType(typeof(KeyValuePair<object, object>));
                }));

        [Fact]
        public void MineShouldBeMappedAndReturnCorrectViewWithoutPageIndex()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Characters/Mine")
                .WithUser()
                .WithAntiForgeryToken())
            .To<CharactersController>(c =>
                c.Mine(With.No<int>()))
            .Which()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<CharacterFullModel>());

    }
}
