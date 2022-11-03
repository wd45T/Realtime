namespace Realtime.Chat.Service.Interfaces
{
    public interface IChatService
    {
        Task<IEnumerable<byte[]>> ReceiveMessagesAsync(Guid clientSessionId);

        Task SendMessageAsync(Guid chatId, byte[] message);
    }
}
