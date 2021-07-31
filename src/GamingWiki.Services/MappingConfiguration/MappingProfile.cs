using AutoMapper;
using GamingWiki.Models;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Services.Models.Categories;
using GamingWiki.Services.Models.Characters;
using GamingWiki.Services.Models.Classes;
using GamingWiki.Services.Models.Comments;
using GamingWiki.Services.Models.Games;

namespace GamingWiki.Services.MappingConfiguration
{
    public class MappingProfile : Profile
    {
        private const string DetailsDateFormat = "f";
        private const string ArticleListingDateFormat = "D";
        private const string CommentDateFormat = "dd/MM/yyyy";

        public MappingProfile()
        {
            //Articles
            this.CreateMap<Article, ArticleServiceDetailsModel>()
                .ForMember(a => a.Author, cfg => cfg.MapFrom(a => a.Author.UserName))
                .ForMember(a => a.PublishedOn, cfg => cfg.MapFrom(a => a.PublishedOn.ToString(DetailsDateFormat)));

            this.CreateMap<Article, ArticleServiceHomeModel>()
                .ForMember(a => a.ShortContent, cfg => cfg.MapFrom(a => a.Content.Substring(0, 200)));

            this.CreateMap<Article, ArticleAllServiceModel>()
                .ForMember(a => a.PublishedOn, cfg => cfg.MapFrom(a => a.PublishedOn.ToString(ArticleListingDateFormat)));

            this.CreateMap<ArticleServiceDetailsModel, ArticleServiceEditModel>();

            //Characters
            this.CreateMap<Character, CharacterServiceDetailsModel>();

            this.CreateMap<Character, CharacterAllServiceModel>();

            this.CreateMap<CharacterServiceDetailsModel, CharacterServiceEditModel>();

            //Categories
            this.CreateMap<Category, CategoryServiceModel>();

            //Classes
            this.CreateMap<Class, ClassSimpleServiceModel>();

            //Comments
            this.CreateMap<Comment, CommentServiceModel>()
                .ForMember(c => c.Commenter, cfg => cfg
                    .MapFrom(c => c.Commenter.UserName))
                .ForMember(c => c.AddedOn, cfg => cfg
                    .MapFrom(c => c.AddedOn.ToString(CommentDateFormat)));

            // Games
            this.CreateMap<Game, GameServiceSimpleModel>();
        }
    }
}
