using AutoMapper;
using GamingWiki.Models;
using GamingWiki.Services.Models.Areas;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Services.Models.Categories;
using GamingWiki.Services.Models.Characters;
using GamingWiki.Services.Models.Classes;
using GamingWiki.Services.Models.Comments;
using GamingWiki.Services.Models.Games;
using GamingWiki.Services.Models.Genres;
using GamingWiki.Services.Models.Platforms;
using GamingWiki.Services.Models.Replies;
using GamingWiki.Services.Models.Reviews;
using GamingWiki.Services.Models.Tricks;

namespace GamingWiki.Services.MappingConfiguration
{
    public class MappingProfile : Profile
    {
        private const string DetailsDateFormat = "f";
        private const string ArticleListingDateFormat = "D";
        private const string CommentDateFormat = "dd/MM/yyyy";

        public MappingProfile()
        {
            //Areas
            this.CreateMap<Area, AreaServiceModel>();

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
            this.CreateMap<Character, CharacterGameServiceModel>();
            this.CreateMap<Character, CharacterPendingModel>()
                .ForMember(c => c.ContributorName, cfg => 
                    cfg.MapFrom(c => c.Contributor.UserName));

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
            this.CreateMap<Game, GameServiceListingModel>();
            this.CreateMap<GameServiceDetailsModel, GameServiceEditModel>();
            this.CreateMap<Game, GameServiceDetailsModel>()
                .ForMember(g => g.Area, cfg => cfg
                    .MapFrom(g => g.Area.Name))
                .ForMember(g => g.Genre, cfg => cfg
                    .MapFrom(g => g.Genre.Name));

            //Genres
            this.CreateMap<Genre, GenreServiceModel>();

            //SupportedPlatforms
            this.CreateMap<Platform, PlatformServiceModel>();

            //Replies
            this.CreateMap<Reply, ReplyServiceModel>()
                .ForMember(r => r.Replier, cfg => cfg
                    .MapFrom(r => r.Replier.UserName));

            //Reviews
            this.CreateMap<Review, ReviewDetailsServiceModel>()
                .ForMember(r => r.Author, cfg => cfg
                    .MapFrom(r => r.Author.UserName));
            
            this.CreateMap<Review, ReviewServiceSimpleModel>()
                .ForMember(r => r.Author, cfg => cfg
                    .MapFrom(r => r.Author.UserName));

            //Tricks
            this.CreateMap<Trick, TrickServiceHomeModel>();
            this.CreateMap<Trick, TrickServiceListingModel>()
                .ForMember(t => t.Author, cfg => cfg
                    .MapFrom(t => t.Author.UserName));
            this.CreateMap<TrickServiceListingModel, TrickServiceEditModel>();
        }
    }
}
