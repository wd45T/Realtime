namespace Realtime.Engine.Repositories.Interfaces
{
    public interface ILongPollingRepository
    {       
        Task<IEnumerable<byte[]>> PopEventsAsync(Guid clientSessionId);

        Task PushEventsAsync(byte[] @event, IEnumerable<Guid> clientsSessionIds);
    }
}
