using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yak.Cap.RabbitMQ.Models;

namespace Yak.Cap.RabbitMQ.DB
{
    public class CapRabbitMQDbContext : DbContext
    {
        public CapRabbitMQDbContext(DbContextOptions<CapRabbitMQDbContext> options)
            : base(options)
        {
        }
        /// <summary>
        /// 重写父类的方法 用于连接数据库
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.;database=CapDb2;uid=sa;pwd=sa123456");
            }
        }
        //public DbSet<Sys_User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
