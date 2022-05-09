using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yak.Cap.RabbitMQ.DB;
using Yak.Cap.RabbitMQ.PublisherApi.Consul;

namespace Yak.Cap.RabbitMQ.PublisherApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration.GetSection("Consul").Get<ConsulOption>());
            services.AddControllers();
            //添加数据库上下文服务
            services.AddDbContext<CapRabbitMQDbContext>();
            //添加事件总线cap
            services.AddCap(x => {
                // 使用内存存储消息(消息发送失败处理)
                //x.UseInMemoryStorage();

                x.UseEntityFramework<CapRabbitMQDbContext>();

                //使用RabbitMQ进行事件中心处理
                x.UseRabbitMQ(rb => {
                    rb.HostName = "localhost";
                    rb.UserName = "guest";
                    rb.Password = "guest";
                    rb.Port = 5672;
                    rb.VirtualHost = "/";
                });

                //var OrderDatabaseString = Configuration.GetConnectionString("OrderDatabaseString");
                //var rabbitMQ = Configuration.GetConnectionString("rabbitMQ");
                //x.UseSqlServer(OrderDatabaseString);
                //x.UseRabbitMQ(rabbitMQ);
                x.FailedRetryCount = 10;//重试最高次数
                x.FailedRetryInterval = 10; //重试间隔
                x.FailedThresholdCallback = Failed =>
                {
                    Console.WriteLine("重试10次");
                };


                //启用仪表盘
                x.UseDashboard();
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddIdentityServerAuthentication(options =>
                 {
                     options.RequireHttpsMetadata = false;
                     options.Authority = "http://localhost:8000"; // 授权服务器地址
                     options.ApiName = "Yak.Cap.RabbitMQ.PublisherApi"; // Api名字
                 });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CapRabbitMQDbContext dataDBContext, IHostApplicationLifetime lifetime, ConsulOption consulOption)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.RegisterConsul(lifetime, consulOption);
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            dataDBContext.Database.EnsureCreated();//数据库不存在的话，会自动创建
            //app.UseCap();
        }
    }
}
