using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights;

namespace ReservationApi
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var _appInsightConfiguration = new TelemetryConfiguration() { InstrumentationKey = "153fa963-4f48-4576-85a8-2076571d33fe" };




            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //.WriteTo.MSSqlServer(Configuration.GetConnectionString("LogConnection"), "_logs", columnOptions: columnOptions)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.ApplicationInsights(_appInsightConfiguration, TelemetryConverter.Events)
            .CreateLogger();

            Log.Information($"Log this Serilog {DateTime.Now} UTC {DateTime.UtcNow}");

            CreateWebHostBuilder(args).Build().Run();
        }


        //WebHost and Generic Host Explained 
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();
    }
}
