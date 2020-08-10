using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Hermes.API.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VersionedApiController : ControllerBase
    {
        protected string GetRemoteIpAddress()
        {
            return HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}