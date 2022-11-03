using Realtime.Engine.Repositories.Interfaces;
using Realtime.Engine.Сonfiguration;
using StackExchange.Redis;

namespace Realtime.Engine.Repositories.Implementations
{
    public class LongPollingByRedisRepository : ILongPollingRepository
    {
        private readonly ISubscriber _subscriber;
        private readonly TimeSpan _expirySession;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private const string _realTimeEventPrefixKey = "RealTimeEvents";

        public LongPollingByRedisRepository(
            IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;

            var endpoint = _connectionMultiplexer.GetEndPoints().FirstOrDefault();

            _subscriber = _connectionMultiplexer.GetSubscriber(endpoint);

            _expirySession = new TimeSpan(0, EngineConfig.ExpiryEventsInMin, 0);
        }

        public async Task<IEnumerable<byte[]>> PopEventsAsync(Guid clientSessionId)
        {
            var realTimeEventKey = GetRealTimeEventKey(clientSessionId);

            var currentEvents = await ListRightPopAsync(realTimeEventKey);

            if (currentEvents.Any()) return currentEvents;

            var cancellationTokenSource = new CancellationTokenSource();

            var channelPattern = $"__keyspace@{GetDatabase.Database}__:{realTimeEventKey}";

            // Use command "config set notify-keyspace-events KEA"
            await _subscriber.SubscribeAsync(channelPattern, async (channel, value) =>
            {
                if (value == "rpush")
                {
                    await _subscriber.UnsubscribeAsync(channelPattern);

                    currentEvents = await ListRightPopAsync(realTimeEventKey);

                    cancellationTokenSource.Cancel();
                }
            });

            try
            {
                await Task.Delay(EngineConfig.LongPollingInMs, cancellationTokenSource.Token);

                await _subscriber.UnsubscribeAsync(channelPattern);
            }
            catch (TaskCanceledException ex) { _ = ex; }

            return currentEvents;
        }

        public async Task PushEventsAsync(byte[] @event, IEnumerable<Guid> clientSessionIds)
        {
            if (@event == default || clientSessionIds == default || !clientSessionIds.Any()) return;

            var tasks = clientSessionIds.Select(async sessionId =>
            {
                var key = GetRealTimeEventKey(sessionId);

                await GetDatabase.ListRightPushAsync(key, @event);

                await GetDatabase.KeyExpireAsync(key, _expirySession);
            });

            await Task.WhenAll(tasks);
        }

        private async Task<IEnumerable<byte[]>> ListRightPopAsync(string realTimeEventKey)
        {
            var events = new List<byte[]>();

            while (true)
            {
                var redisValue = await GetDatabase.ListRightPopAsync(realTimeEventKey);

                if (!redisValue.HasValue) break;

                events.Add(redisValue);
            }

            return events;
        }

        private static string GetRealTimeEventKey(Guid clientSessionId) => $"{_realTimeEventPrefixKey}:{clientSessionId}";

        private IDatabase GetDatabase => _connectionMultiplexer.GetDatabase();
    }
}
