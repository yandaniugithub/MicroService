using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Yak.Ocelot.Gateway
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("/healthCheck")]
        public IActionResult Check() => Ok("ok");
    }
}
