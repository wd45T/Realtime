namespace Realtime.Engine.Services
{
    /// <summary>
    /// Provides real-time methods clients communication.
    /// </summary>
    public interface IRealTimeEventService
    {
        /// <summary>
        /// Get events belonging to the session.
        /// </summary>
        /// <param name="clientSessionId">Client session ID</param>
        /// <returns>Events for the client.</returns>
        Task<IEnumerable<byte[]>> PopEventsAsync(Guid clientSessionId);

        /// <summary>
        /// Send the event to the listed clients.
        /// </summary>
        /// <param name="event">Event for the clients</param>
        /// <param name="clientSessionIds">Clients session IDs</param>
        Task PushEventsAsync(byte[] @event, IEnumerable<Guid> clientSessionIds);
    }
}