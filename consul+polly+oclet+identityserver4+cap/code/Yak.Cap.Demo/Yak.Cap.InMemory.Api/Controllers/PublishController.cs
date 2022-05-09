using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Yak.Cap.InMemory.Api.Controllers
{
    public class PublishController : Controller
    {
        [Route("~/send")]
        public IActionResult SendMessage([FromServices] ICapPublisher capBus)
        {
            capBus.Publish("test.show.time", DateTime.Now);

            return Ok();
        }

        [Route("~/CapHeaderSend")]
        public IActionResult CapHeaderSendMessage([FromServices] ICapPublisher capBus)
        {
            var header = new Dictionary<string, string>()
            {
                ["my.header.first"] = "first",
                ["my.header.second"] = "second"
            };

            capBus.Publish("test.CapHeadershow.time", DateTime.Now, header);

            return Ok();
        }
    }
}
