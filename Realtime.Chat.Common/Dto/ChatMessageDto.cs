using MessagePack;

namespace Realtime.Chat.Common.Dto
{
    [MessagePackObject(true)]
    public class ChatMessageDto
    {
        public Guid ChatId { get; set; }
        public string? Message { get; set; }
        public DateTime SendingTime { get; set; } = DateTime.UtcNow;
        public Guid SenderSessionId { get; set; }
    }
}
