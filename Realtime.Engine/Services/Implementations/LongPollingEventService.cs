using Realtime.Engine.Repositories.Interfaces;

namespace Realtime.Engine.Services.Implementations
{
    /// <summary>
    /// Implements real-time communication using Long Polling technology.
    /// </summary>
    public class LongPollingEventService : IRealTimeEventService
    {
        private readonly ILongPollingRepository _longPollingRepository;

        public LongPollingEventService(
            ILongPollingRepository longPollingRepository)
        {
            _longPollingRepository = longPollingRepository;
        }

        public async Task<IEnumerable<byte[]>> PopEventsAsync(Guid clientSessionId)
        {
            var events = await _longPollingRepository.PopEventsAsync(clientSessionId);

            return events;
        }

        public async Task PushEventsAsync(byte[] @event, IEnumerable<Guid> clientSessionIds)
        {
            await _longPollingRepository.PushEventsAsync(@event, clientSessionIds);
        }
    }
}