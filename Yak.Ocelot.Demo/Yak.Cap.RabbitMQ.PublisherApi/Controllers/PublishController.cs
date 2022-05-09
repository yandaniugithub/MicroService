using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yak.Cap.RabbitMQ.DB;
using Yak.Cap.RabbitMQ.Models;

namespace Yak.Cap.RabbitMQ.PublisherApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PublishController : ControllerBase
    {
        private readonly ILogger<PublishController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICapPublisher _capBus;

        public PublishController(ILogger<PublishController> logger, ICapPublisher capPublisher, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _capBus = capPublisher;
        }
        [HttpGet]
        public IActionResult Get()
        {
            string result = $"【订单服务】{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}——" +
                $"{Request.HttpContext.Connection.LocalIpAddress}:{_configuration["Consul:ServicePort"]}";
            return Ok(result);
        }
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        [Route("Create")]
        [HttpPost]
        public async Task<string> CreateOrder([FromServices] CapRabbitMQDbContext dbContext)
        {

            var orderId = DateTime.Now.ToString("yyyyMMdd") + new Random().Next(0, 100000000);
            var order = new Order()
            {
                Id = orderId,
                SkuName = "测试商品",
                SkuId = 78956426666244,
                Num = 60
            };
            var headers = new Dictionary<string, string>();
            headers.Add("Buy", "24");

            /*消息接收方名称*/
            var PublishName = "ProductCheckHouseNum";
            /*推送内容*/
            var PublishContent = order;
            /*推送Header*/
            var PublishHeader = headers;

            using (var trans = dbContext.Database.BeginTransaction(_capBus, autoCommit: false))
            {
                //业务代码
                try
                {
                    dbContext.Add<Order>(order);
                    var result = dbContext.SaveChanges();
                    await _capBus.PublishAsync(PublishName, PublishContent, PublishHeader);
                    if (result > 0) trans.Commit();
                    else trans.Rollback();
                    orderId = result > 0 ? orderId : "";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
                
            }
            return orderId;
        }
    }
}
