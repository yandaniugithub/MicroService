using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yak.Ocelot.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((context, config) =>
               {
                   config.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
               })
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseUrls("http://localhost:5000");
                   webBuilder.UseStartup<Startup>();
               });
    }
}
