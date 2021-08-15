namespace GamingWiki.Services.Contracts
{
    public interface IMessageService
    {
        bool Delete(int messageId);

        bool MessageExists(int messageId);

        int GetDiscussionId(int messageId);

        string GetSenderId(int messageId);
    }
}
