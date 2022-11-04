using Realtime.Chat.Common.Dto;
using Realtime.Chat.Common.TransportLayer.Utilities;
using Realtime.Chat.Service.Interfaces;
using Realtime.Engine.Services;

namespace Realtime.Chat.Service.Implementations
{
    public class ChatService : IChatService
    {
        private readonly IDataConverter _dataConverter;
        private readonly IRealTimeEventService _realTimeEventService;

        // TODO implement a bunch of sessions with chats.
        private static readonly Dictionary<Guid, List<Guid>> _chatSessions;

        static ChatService()
        {
            _chatSessions = new Dictionary<Guid, List<Guid>>();
        }

        public ChatService(
            IDataConverter dataConverter,
            IRealTimeEventService realTimeEventService)
        {
            _dataConverter = dataConverter;
            _realTimeEventService = realTimeEventService;
        }

        public async Task<IEnumerable<ChatMessageDto>> ReceiveMessagesAsync(Guid clientSessionId)
        {
            var messagesInBytes = await _realTimeEventService.PopEventsAsync(clientSessionId);

            var messages = messagesInBytes.Select(x => _dataConverter.BytesToObject<ChatMessageDto>(x));

            return messages;
        }

        public async Task SendMessageAsync(ChatMessageDto chatMessage)
        {
            _chatSessions.TryGetValue(chatMessage.ChatId, out var clientsSessionIds);

            if (clientsSessionIds == default) return;

            var message = _dataConverter.ObjectToBytes(chatMessage);

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
