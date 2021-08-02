using GamingWiki.Services.Models.Tricks;

namespace GamingWiki.Web.Models.Tricks
{
    public class TrickFullModel : BaseFullModel
    {
        public PaginatedList<TrickServiceListingModel> Tricks { get; set; }
    }
}
