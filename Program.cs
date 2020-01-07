using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
           

            CreateWebHostBuilder(args).Build().Run();
            //Session 2
            var _appInsightConfiguration = new TelemetryConfiguration() { InstrumentationKey = "153fa963-4f48-4576-85a8-2076571d33fe" };



            //Session 2
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //.WriteTo.MSSqlServer(Configuration.GetConnectionString("LogConnection"), "_logs", columnOptions: columnOptions)
            .Enrich.FromLogContext()
            // Log to Console provider
            .WriteTo.Console()
            //Log to Application Insights Provider 
            //App Insights configuration and Converter Type set to Event 
            .WriteTo.ApplicationInsights(_appInsightConfiguration, TelemetryConverter.Events)
            .CreateLogger();


            // Serilog Log Info 
            Log.Information($"Log this Serilog {DateTime.Now} UTC {DateTime.UtcNow}");


        }


        //WebHost and Generic Host Explained 
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                if (context.HostingEnvironment.IsDevelopment())
                {
                    var builtConfig = config.Build();

                    using (var store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
                    {
                        store.Open(OpenFlags.ReadOnly);
                        var certs = store.Certificates
                            .Find(X509FindType.FindByThumbprint,
                                builtConfig["AzureADCertThumbprint"], false);

                        config.AddAzureKeyVault(
                            $"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
                            builtConfig["AzureADApplicationId"],
                            certs.OfType<X509Certificate2>().Single());

                        store.Close();
                       
                    }
                   
                }
            })
                .UseStartup<Startup>()
                .UseSerilog();
    }
}
