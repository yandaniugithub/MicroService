using Microsoft.AspNetCore.Mvc;

namespace Yak.Cap.RabbitMQ.SubscribeApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("/healthCheck")]
        public IActionResult Check() => Ok("ok");
    }
}
