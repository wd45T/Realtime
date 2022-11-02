namespace Realtime.Engine.Services.Implementations
{
    /// <summary>
    /// Implements real-time communication using Long Polling technology.
    /// </summary>
    public class LongPollingEventService : IRealTimeEventService
    {
        public LongPollingEventService()
        {

        }

        public async Task<IEnumerable<byte[]>> PopEventsAsync(Guid clientSessionId)
        {
            await Task.CompletedTask;

            throw new NotImplementedException();
        }

        public async Task PushEventsAsync(byte[] @event, IEnumerable<Guid> clientSessionIds)
        {
            await Task.CompletedTask;

            throw new NotImplementedException();
        }
    }
}