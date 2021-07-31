using System;

namespace GamingWiki.Web.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string Message { get; set; } 
            = "An error occurred while processing your request.";
    }
}
