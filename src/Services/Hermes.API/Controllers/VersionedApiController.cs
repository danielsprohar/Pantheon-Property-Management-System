using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VersionedApiController : ControllerBase
    {
        protected string GetRemoteIpAddress()
        {
            return HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}