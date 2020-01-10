using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ReservationApi.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            string[] corsDomains = configuration.GetSection("CorsDomains")
                .Get<string>()
                .Split(',');

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", builder =>
                {
                    builder
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .WithOrigins(corsDomains)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .Build();
                });
            });
        }
    }
}
