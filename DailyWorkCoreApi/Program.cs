using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace DailyWorkCoreApi
{
    public class Program
    {
        public static void Main(string[] args)
        {  
            // ------------- Adding Serilog -------------
            Log.Logger = new LoggerConfiguration()
                // .MinimumLevel.Debug() 
                .WriteTo.Console()
                .WriteTo.File("logs\\DailyWorkCoreApi-log-.txt", rollingInterval: RollingInterval.Hour)
                .CreateLogger();
            // ------------- Adding Serilog -------------

            CreateHostBuilder(args).Build().Run();

            Log.CloseAndFlush();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog();
    }
}
