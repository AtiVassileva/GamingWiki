using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamingWiki.Models;
using GamingWiki.Web.Models.Characters;
using GamingWiki.Web.Models.Games;

namespace GamingWiki.Web.MappingConfiguration
{
    public class GamingWikiProfile : Profile
    {
        public GamingWikiProfile()
        {
            this.CreateMap<GameDtoModel, Game>();

            this.CreateMap<GameCreatorDto, GameCreator>();

            this.CreateMap<CharacterDtoModel, Character>();
        }
    }
}
