using Microsoft.AspNetCore.Mvc;
using Realtime.Chat.Service.Interfaces;

namespace Realtime.Chat.API.Controllers
{
    public class ChatController : ControllerBase
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
            await Task.CompletedTask;

            return Ok("Hello, World!");
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessageAsync([FromBody] byte[] request)
        {
            await Task.CompletedTask;

            return Ok("Message sent, maybe");
        }
    }
}