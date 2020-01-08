using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ReservationApi.Data.EFCore.Contexts;
using ReservationApi.Data.EFCore.Repos;
using ReservationApi.Data.Intefaces;
using ReservationApi.Data.Models;

namespace ReservationApi.Data.EFCore.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSqlDatabaseSupport(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ReservationContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.TryAddScoped<DbContext, ReservationContext>();
            services.TryAddScoped<IRepository<Reservation>, EFRepository<Reservation>>();
        }

        //private static void 
    }
}
