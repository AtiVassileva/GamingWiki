﻿namespace GamingWiki.Services.Models.Messages
{
    public class MessageServiceModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string SenderId { get; set; }

        public string Sender { get; set; }

        public string SentOn { get; set; }
    }
}
