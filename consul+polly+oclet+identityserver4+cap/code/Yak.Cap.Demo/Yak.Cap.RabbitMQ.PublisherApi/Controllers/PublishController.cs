using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yak.Cap.RabbitMQ.DB;
using Yak.Cap.RabbitMQ.Models;

namespace Yak.Cap.RabbitMQ.PublisherApi.Controllers
{
    public class PublishController : Controller
    {
        private readonly ICapPublisher _capBus;

        public PublishController(ICapPublisher capPublisher)
        {
            _capBus = capPublisher;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        [Route("~/CreateOrder")]
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
            headers.Add("Buy", "234");

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
