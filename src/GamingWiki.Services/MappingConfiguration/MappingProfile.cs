using AutoMapper;
using GamingWiki.Models;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Services.Models.Categories;

namespace GamingWiki.Services.MappingConfiguration
{
    public class MappingProfile : Profile
    {
        private const string DetailsDateFormat = "f";
        private const string ArticleListingDateFormat = "D";

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

            //Categories
            this.CreateMap<Category, CategoryServiceModel>();
        }
    }
}
