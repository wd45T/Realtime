using Realtime.Chat.Common.Dto;

namespace Realtime.Chat.Service.Interfaces
{
    public interface IChatService
    {
        Task<IEnumerable<ChatMessageDto>> ReceiveMessagesAsync(Guid clientSessionId);

        Task SendMessageAsync(ChatMessageDto chatMessage);

        Task SubscribeToChatAsync(Guid clientSessionId, Guid chatId);
    }
}
