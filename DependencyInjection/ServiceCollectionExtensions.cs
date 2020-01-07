using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using ReservationApi.Contexts;
using ReservationApi.Models;
using ReservationApi.Repos;

namespace ReservationApi.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMongoDatabaseSupport(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMongoCollection<Reservation>>(sp =>
            {
                var config = configuration.GetSection("ReservationDataBaseSettings");

                var client = new MongoClient(config.GetValue<string>("ConnectionString"));
                var database = client.GetDatabase(config.GetValue<string>("DatabaseName"));

                return database.GetCollection<Reservation>(config.GetValue<string>("ReservationCollectionName"));
            });

            services.TryAddScoped<IRepository<Reservation>, MongoRepository<Reservation>>();
        }

        public static void AddEFCoreDatabaseSupport(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ReservationContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.TryAddScoped<DbContext, ReservationContext>();
            services.TryAddScoped<IRepository<Reservation>, EFRepository<Reservation>>();
        }
    }
}
