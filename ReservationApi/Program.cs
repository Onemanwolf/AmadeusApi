using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ReservationApi
{
    public class Program
    {
        public static void Main(string[] args)
        {

            CreateWebHostBuilder(args).Build().Run();

        }


        //WebHost and Generic Host Explained 
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {

                    //Toggle for development and production 
                    if (context.HostingEnvironment.IsProduction())
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
                .UseStartup<Startup>();
    }
}
