using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yak.Cap.RabbitMQ.Models;

namespace Yak.Cap.RabbitMQ.SubscribeApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConsumerController : ControllerBase
    {
        private readonly ILogger<ConsumerController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICapPublisher _capBus;

        public ConsumerController(ILogger<ConsumerController> logger, IConfiguration configuration, ICapPublisher capPublisher)
        {
            _logger = logger;
            _configuration = configuration;
            _capBus = capPublisher;
        }
        [HttpGet]
        public IActionResult Get()
        {
            string result = $"【产品服务】{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}——" +
                $"{Request.HttpContext.Connection.LocalIpAddress}:{_configuration["ConsulSetting:ServicePort"]}";
            return Ok(result);
        }
        /// <summary>
        ///  接收下单库存验证消息,减库存 订阅下单事件
        /// </summary>
        /// <param name="Order"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        [NonAction]
        [CapSubscribe("ProductCheckHouseNum")]
        public Order ProductCheckHouseNum(Order Order, [FromCap] CapHeader header)
        {

            if (Order != null)
            {
                Console.WriteLine(Order.SkuName);
            }
            return Order;

        }

    }
}
