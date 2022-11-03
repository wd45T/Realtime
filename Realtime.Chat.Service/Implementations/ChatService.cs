using Realtime.Chat.Service.Interfaces;
using Realtime.Engine.Services;

namespace Realtime.Chat.Service.Implementations
{
    public class ChatService : IChatService
    {
        private readonly IRealTimeEventService _realTimeEventService;

        public ChatService(
            IRealTimeEventService realTimeEventService)
        {
            _realTimeEventService = realTimeEventService;
        }

        public async Task<IEnumerable<byte[]>> ReceiveMessagesAsync(Guid clientSessionId)
        {
            var messages = await _realTimeEventService.PopEventsAsync(clientSessionId);

            return messages;
        }

        public async Task SendMessageAsync(Guid chatId, byte[] message)
        {
            // Достать все сессии chatId.
            var clientsSessionIds = new List<Guid> { Guid.Parse("b4b352cf-309d-4505-9e57-a6306cb8615e") };

            await _realTimeEventService.PushEventsAsync(message, clientsSessionIds);
        }
    }
}
