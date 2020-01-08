using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using ReservationApi.Data.Intefaces;
using ReservationApi.Data.Models;
using ReservationApi.Data.MongoDb.Repos;

namespace ReservationApi.Data.MongoDb.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMongoDatabaseSupport(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMongoCollection<Reservation>>(sp =>
            {
                var config = configuration.GetSection("ReservationDataBaseSettings");

                var client = new MongoClient(config["ConnectionString"]);
                var database = client.GetDatabase(config["DatabaseName"]);

                return database.GetCollection<Reservation>(config["ReservationCollectionName"]);
            });

            services.TryAddScoped<IRepository<Reservation>, MongoRepository<Reservation>>();
        }
    }
}
