﻿using System;
using System.Linq;
using GamingWiki.Models;
using GamingWiki.Services.Models.Characters;
using GamingWiki.Web.Controllers;
using GamingWiki.Web.Models.Characters;
using static GamingWiki.Tests.Data.Characters;
using static GamingWiki.Tests.Data.Classes;
using static GamingWiki.Tests.Data.Games;
using static GamingWiki.Web.Common.WebConstants;
using static GamingWiki.Web.Areas.Admin.AdminConstants;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace GamingWiki.Tests.Controllers
{
    public class CharacterControllerTest
    {
        private const int DefaultPageIndex = 1;

        [Fact]
        public void CharactersControllerShouldBeForAuthorizedUsersOnly()
        => MyController<CharactersController>
            .ShouldHave()
            .Attributes(attributes => attributes
                .RestrictingForAuthorizedRequests());

        [Fact]
        public void AllShouldReturnCorrectViewWithModel()
            => MyController<CharactersController>
                .Instance(instance =>
                    instance.WithData(FiveCharacters))
                .Calling(c => c.All(DefaultPageIndex))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CharacterFullModel>());

        [Fact]
        public void GetCreateShouldReturnCorrectView()
            => MyController<CharactersController>
                .Instance()
                .Calling(c => c.Create())
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CharacterFormModel>());

        [Fact]
        public void PostCreateReturnRedirectWithValidModel()
            => MyController<CharactersController>
                .Instance(controller => controller
                    .WithData(TestCharacterClass)
                    .WithData(TestGame))
                .Calling(c => c.Create(TestCharacterFormModel))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                    .WithSet<Character>(characters =>
                        characters.Any(c =>
                        c.Name == TestCharacterFormModel.Name
                        && c.Description == TestCharacterFormModel.Description)
                    ))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<CharactersController>
                        (c => c.Details(With.Any<int>())));

        [Fact]
        public void PostCreateShouldReturnViewWithInvalidModelState()
            => MyController<CharactersController>
                .Instance(controller => controller
                    .WithUser(TestUser.Username, TestUser.Identifier))
                .Calling(c => c.Create(TestCharacterFormModel))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CharacterFormModel>());

        [Fact]
        public void DetailsShouldReturnErrorViewWithInvalidCharacterId()
            => MyController<CharactersController>
                .Instance()
                .Calling(c => c.Details(new Random().Next()))
                .ShouldReturn()
                .View("Error");

        [Fact]
        public void DetailsShouldReturnCorrectViewWithValidCharacterId()
            => MyController<CharactersController>
                .Instance(instance => instance
                    .WithData(TestCharacter))
                .Calling(c => c.Details(With.Value(TestCharacter.Id)))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CharacterServiceDetailsModel>());

        [Fact]
        public void GetEditShouldReturnErrorViewWithInvalidId()
            => MyController<CharactersController>
                .Instance()
                .Calling(c => c.Edit(new Random().Next()))
                .ShouldReturn()
                .View("Error");

        [Fact]
        public void GetEditShouldReturnCorrectViewWithValidId()
            => MyController<CharactersController>
                .Instance(instance => instance
                    .WithUser(user => user
                        .InRole(AdministratorRoleName))
                    .WithData(TestCharacter))
                .Calling(c => c.Edit(TestCharacter.Id))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CharacterServiceEditModel>());

        [Fact]
        public void GetEditShouldReturnUnauthorizedForUnauthorizedUsers()
            => MyController<CharactersController>
                .Instance(instance => instance
                    .WithData(TestCharacter))
                .Calling(c => c.Edit(TestCharacter.Id))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void PostEditShouldReturnRedirectWithValidModel()
            => MyController<CharactersController>
                .Instance(controller => controller
                    .WithData(TestCharacter)
                    .WithUser(TestUser.Username, TestUser.Identifier))
                .Calling(c => c.Edit(TestValidCharacterEditModel, TestCharacter.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .ValidModelState()
                .Data(data => data
                    .WithSet<Character>(characters => characters
                        .Any(c =>
                            c.Name == TestCharacter.Name
                            && c.Description == TestCharacter.Description)
                    )).TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<CharactersController>(c =>
                        c.Details(With.Any<int>())));

        [Fact]
        public void PostEditShouldReturnViewWithInvalidModelState()
            => MyController<CharactersController>
                .Instance(controller => controller
                    .WithData(TestCharacter)
                    .WithUser(TestUser.Username, TestUser.Identifier))
                .Calling(c => c.Edit(TestInvalidCharacterEditModel, TestCharacter.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CharacterServiceEditModel>());

        [Fact]
        public void PostEditShouldReturnBadRequestUponUnsuccessfulEdition()
            => MyController<CharactersController>
                .Instance(controller => controller
                    .WithData(TestCharacterClass)
                    .WithUser(TestUser.Username, TestUser.Identifier))
                .Calling(c => c.Edit(TestValidCharacterEditModel, TestCharacter.Id))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .AndAlso()
                .ShouldReturn()
                .BadRequest();

        [Fact]
        public void DeleteShouldReturnErrorViewWithInvalidCharacterId()
            => MyController<CharactersController>
                .Instance()
                .Calling(c => c.Delete(TestCharacter.Id))
                .ShouldReturn()
                .View("Error");

        [Fact]
        public void DeleteShouldReturnUnauthorizedForUnauthorizedUsers()
            => MyController<CharactersController>
                .Instance(instance => instance
                    .WithData(TestCharacter))
                .Calling(c => c.Delete(TestCharacter.Id))
                .ShouldReturn()
                .Unauthorized();

        [Fact]
        public void DeleteShouldReturnRedirectWithValidId()
            => MyController<CharactersController>
                .Instance(controller => controller
                    .WithData(TestCharacter)
                    .WithUser(user => user
                        .InRole(AdministratorRoleName)))
                .Calling(c => c.Delete(TestCharacter.Id))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Character>(characters => !characters
                        .Any(c =>
                            c.Name == TestCharacterFormModel.Name
                            && c.Description == TestCharacterFormModel.Name)
                    )).TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<CharactersController>(c =>
                        c.All(With.Any<int>())));

        [Fact]
        public void GetSearchShouldReturnCorrectViewWithModel()
            => MyController<CharactersController>
                .Instance(instance =>
                    instance.WithData(FiveCharacters))
                .Calling(c => c.Search(Guid.NewGuid()
                    .ToString(), DefaultPageIndex))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CharacterFullModel>());

        [Fact]
        public void FilterShouldReturnCorrectViewWithModel()
            => MyController<CharactersController>
                .Instance(instance =>
                    instance.WithData(FiveCharacters))
                .Calling(c => c.Filter(new Random().Next(), DefaultPageIndex))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CharacterFullModel>());

        [Fact]
        public void MineShouldReturnCorrectViewWithModel()
            => MyController<CharactersController>
                .Instance(instance =>
                    instance.WithData(FiveCharacters))
                .Calling(c => c.Mine(DefaultPageIndex))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CharacterFullModel>());
    }
}
