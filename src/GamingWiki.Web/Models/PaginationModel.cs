using System.Collections.Generic;
using GamingWiki.Services.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GamingWiki.Web.Models
{
    public class PaginationModel
    {
        public PaginatedList<object> PaginatedList  { get; set; }
        
        public string ControllerName { get; set; }

        public KeyValuePair<object, object> Tokens { get; set; }
    }
}
