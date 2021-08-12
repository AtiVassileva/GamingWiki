using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GamingWiki.Models;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.Models.Messages;
using GamingWiki.Web.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace GamingWiki.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IDiscussionService discussionService;
        public ChatHub(IDiscussionService discussionService) 
            => this.discussionService = discussionService;

        public async Task Send(string message, string discussionId)
        {
            var discussionParsedId = int.Parse(discussionId);
            var senderId = this.Context.User.GetId();

            this.discussionService.AddMessage(discussionParsedId, content: message, senderId);

            await this.Clients.All.SendAsync(
                "NewMessage",
                new MessageServiceModel
                {
                    SenderId = senderId,
                    Content = message,
                    SentOn = DateTime.UtcNow.ToString("t")
                });
        }
    }
}
