namespace Realtime.Engine.Сonfiguration
{
    public static class EngineConfig
    {
        /// <summary>
        /// Event lifetime
        /// </summary>
        public static int ExpiryEventsInMin { get; set; } = 5;

        /// <summary>
        /// Waiting time for long polling
        /// </summary>
        public static int LongPollingInMs { get; set; } = 10000;
    }
}
