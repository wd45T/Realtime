using Microsoft.AspNetCore.Mvc;
using Realtime.Chat.Common.TransportLayer;

namespace Realtime.Chat.API.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected Guid GetClientSessionId()
        {
            _ = Guid.TryParse(Request.Headers[Headers.ClientSessionId], out var clientSessionId);

            return clientSessionId;
        }
    }
}
