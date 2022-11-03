using Microsoft.AspNetCore.Mvc;
using Realtime.Chat.Common.TransportLayer.Commands.Request;
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
            var clientSessionId = GetClientSessionId();

            if (clientSessionId == default) return BadRequest("Not found client session ID.");

            var bytes = await _chatService.ReceiveMessagesAsync(clientSessionId);

            var messages = bytes.Select(x => Encoding.UTF8.GetString(x));

            return Ok(messages);
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessageAsync([FromBody] SendMessageRequest request)
        {
            await _chatService.SendMessageAsync(request.ChatId, Encoding.UTF8.GetBytes(request.Message));

            return Ok();
        }

        [HttpPost("SubscribeToChat")]
        public async Task<IActionResult> SubscribeToChatAsync([FromBody] SubscribeToChatRequest request)
        {
            var clientSessionId = GetClientSessionId();

            if (clientSessionId == default) return BadRequest("Not found client session ID.");

            await _chatService.SubscribeToChatAsync(clientSessionId, request.ChatId);

            return Ok();
        }
    }
}
