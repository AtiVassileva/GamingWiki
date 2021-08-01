using System.Collections.Generic;
using GamingWiki.Services.Models.Reviews;

namespace GamingWiki.Web.Models.Reviews
{
    public class ReviewFullModel
    {
        public PaginatedList<ReviewDetailsServiceModel> Reviews { get; set; }

        public KeyValuePair<object, object> Tokens { get; set; }
    }
}
