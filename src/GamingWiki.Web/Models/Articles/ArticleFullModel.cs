﻿using System.Collections.Generic;
using GamingWiki.Services.Models.Articles;
using GamingWiki.Services.Models.Categories;

namespace GamingWiki.Web.Models.Articles
{
    public class ArticleFullModel
    {
        public IEnumerable<ArticleAllServiceModel> Articles { get; set; }

        public IEnumerable<CategoryServiceModel> Categories { get; set; }
    }
}
