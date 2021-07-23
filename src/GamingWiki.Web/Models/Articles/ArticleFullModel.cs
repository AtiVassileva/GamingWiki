using System.Collections.Generic;
using GamingWiki.Web.Models.Categories;

namespace GamingWiki.Web.Models.Articles
{
    public class ArticleFullModel
    {
        public IEnumerable<ArticleSimpleModel> Articles { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}
