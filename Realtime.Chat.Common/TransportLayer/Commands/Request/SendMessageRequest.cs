namespace Realtime.Chat.Common.TransportLayer.Commands.Request
{
    public class SendMessageRequest
    {
        public Guid ChatId { get; set; }
        public string Message { get; set; }
    }
}
