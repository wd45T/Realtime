using Realtime.Chat.Service.Interfaces;
using Realtime.Engine.Services;

namespace Realtime.Chat.Service.Implementations
{
    public class ChatService : IChatService
    {
        private readonly IRealTimeEventService _realTimeEventService;

        // TODO implement a bunch of sessions with chats.
        private static readonly Dictionary<Guid, List<Guid>> _chatSessions;

        static ChatService()
        {
            _chatSessions = new Dictionary<Guid, List<Guid>>();
        }

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
            _chatSessions.TryGetValue(chatId, out var clientsSessionIds);

            if (clientsSessionIds == default) return;

            await _realTimeEventService.PushEventsAsync(message, clientsSessionIds);
        }

        public async Task SubscribeToChatAsync(Guid clientSessionId, Guid chatId)
        {
            await Task.CompletedTask;

            if (_chatSessions.TryGetValue(chatId, out var clientsSessionIds))
            {
                clientsSessionIds.Add(clientSessionId);
            }
            else
            {
                clientsSessionIds = new List<Guid> { clientSessionId };

                _chatSessions.TryAdd(chatId, clientsSessionIds);
            }
        }
    }
}
