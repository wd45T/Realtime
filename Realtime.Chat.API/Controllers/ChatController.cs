using Microsoft.AspNetCore.Mvc;
using Realtime.Chat.Service.Interfaces;
using System.Text;

namespace Realtime.Chat.API.Controllers
{
    public class ChatController : BaseController
    {
        private readonly IChatService _chatService;

        public ChatController(
            IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("ReceiveMessages")]
        public async Task<IActionResult> ReceiveMessagesAsync()
        {
            //var clientSessionId = GetClientSessionId();

            var clientSessionId = Guid.Parse("b4b352cf-309d-4505-9e57-a6306cb8615e");

            if (clientSessionId == default) return BadRequest("Not found client session ID.");

            var bytes = await _chatService.ReceiveMessagesAsync(clientSessionId);

            var messages = bytes.Select(x => Encoding.ASCII.GetString(x));

            return Ok(messages);
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessageAsync([FromBody] string message)
        {
            var chatId = Guid.NewGuid();

            await _chatService.SendMessageAsync(chatId, Encoding.ASCII.GetBytes(message));

            return Ok();
        }
    }
}
