using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yak.Cap.RabbitMQ.Models;

namespace Yak.Cap.InMemory.Api.Controllers
{
    public class ConsumerController : Controller
    {
        /// <summary>
        ///  接收下单库存验证消息
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
