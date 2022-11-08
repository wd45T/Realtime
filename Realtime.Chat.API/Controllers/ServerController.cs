using Microsoft.AspNetCore.Mvc;

namespace Realtime.Chat.API.Controllers
{
    public class ServerController : BaseController
    {
        [HttpGet("Ping")]
        public async Task<IActionResult> PingAsync()
        {
            await Task.CompletedTask;

            return Ok();
        }
    }
}
