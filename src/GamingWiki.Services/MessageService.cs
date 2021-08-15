using System;
using System.Linq;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Services.Contracts;

namespace GamingWiki.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext dbContext;

        public MessageService(ApplicationDbContext dbContext) 
            => this.dbContext = dbContext;

        public bool Delete(int messageId)
        {
            if (!this.MessageExists(messageId))
            {
                return false;
            }

            var message = this.FindMessage(messageId);

            this.dbContext.Messages.Remove(message);
            this.dbContext.SaveChanges();

            return true;
        }

        public bool MessageExists(int messageId)
            => this.dbContext.Messages
                .Any(m => m.Id == messageId);

        public int GetDiscussionId(int messageId)
            => this.dbContext.Messages
                .Where(m => m.Id == messageId)
                .Select(m => m.DiscussionId)
                .FirstOrDefault();

        public string GetSenderId(int messageId)
            => this.dbContext.Messages
                .Where(m => m.Id == messageId)
                .Select(m => m.SenderId)
                .First();

        private Message FindMessage(int messageId)
            => this.dbContext.Messages
                .First(m => m.Id == messageId);
    }
}
