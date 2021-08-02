using System.Collections.Generic;
using GamingWiki.Services.Models.Reviews;

namespace GamingWiki.Web.Models.Reviews
{
    public class ReviewFullModel : BaseFullModel
    {
        public PaginatedList<ReviewDetailsServiceModel> Reviews { get; set; }
    }
}
