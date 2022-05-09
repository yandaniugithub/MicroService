using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Yak.Cap.RabbitMQ.PublisherApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("/healthCheck")]
        public IActionResult Check() => Ok("ok");
    }
}
