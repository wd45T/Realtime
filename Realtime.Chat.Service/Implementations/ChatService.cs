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
    }
}
