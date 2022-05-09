using DotNetCore.CAP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using Yak.Cap.RabbitMQ.Models;

namespace Yak.Microservice.Product.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICapPublisher _capBus;

        public ProductController(ILogger<ProductController> logger, IConfiguration configuration, ICapPublisher capPublisher)
        {
            _logger = logger;
            _configuration = configuration;
            _capBus = capPublisher;
        }
        [HttpGet]
        public IActionResult Get()
        {
            string result = $"【产品服务】{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}——" +
                $"{Request.HttpContext.Connection.LocalIpAddress}:{_configuration["Consul:ServicePort"]}";
            return Ok(result);
        }
    }
}
