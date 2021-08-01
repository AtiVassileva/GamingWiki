using System.Collections.Generic;
using GamingWiki.Services.Models.Tricks;

namespace GamingWiki.Web.Models.Tricks
{
    public class TrickFullModel
    {
        public PaginatedList<TrickServiceListingModel> Tricks { get; set; }

        public KeyValuePair<object, object> Tokens { get; set; }
    }
}
