using Microsoft.AspNetCore.Mvc;

namespace Realtime.Chat.API.Controllers
{
    public class ChatController : ControllerBase
    {

        [HttpGet("LongPolling")]
        public async Task<IActionResult> LongPollingAsync()
        {
            await Task.CompletedTask;

            return Ok("Hello, World!");
        }
    }
}